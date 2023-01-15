using System.Net.WebSockets;
using System.Text.Json;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.WebSocket;

internal class WebSocketStateChangesListener : IStateChangesListener, IDisposable
{
    private const int BufferSize = 1 << 9;
    
    private readonly Uri _wsRoute;
    private readonly ClientWebSocket _wsClient = new();
    
    public WebSocketStateChangesListener(IConfiguration configuration)
    {
        var localhostPort = configuration.GetValue<int>("LocalHostWsPort");
        _wsRoute = new Uri($"ws://127.0.0.1:{localhostPort}");
    }

    public Task ConnectAsync() => _wsClient.ConnectAsync(_wsRoute, CancellationToken.None);

    /// <summary>
    /// Получает все изменения состояний полученные по WS
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены операции</param>
    /// <returns>Коллекция изменений состояний</returns>
    /// <exception cref="InvalidOperationException">Вызывается, если клиент не подключен к сокету или передан неверный тип WS сообщения</exception>
    public async Task<IEnumerable<UniversityServiceChangeStateEntity>> TryGetChangesAsync(CancellationToken cancellationToken)
    {
        if (_wsClient.State != WebSocketState.Open)
        {
            throw new InvalidOperationException("Can't get new update because WS connection closed");
        }
           
        var buffer = new byte[BufferSize];
        await using var outputStream = new MemoryStream(BufferSize);
        var receiveResult = await _wsClient.ReceiveAsync(buffer, cancellationToken);

        if (receiveResult.MessageType != WebSocketMessageType.Text)
        {
            throw new InvalidOperationException("Expected text WS message type");
        }

        await outputStream.WriteAsync(buffer, 0, receiveResult.Count, cancellationToken);
        return await JsonSerializer.DeserializeAsync<UniversityServiceChangeStateEntity[]>(outputStream, cancellationToken: cancellationToken) ?? Array.Empty<UniversityServiceChangeStateEntity>();
    }

    public void Dispose()
    {
        _wsClient.Dispose();
    }
}