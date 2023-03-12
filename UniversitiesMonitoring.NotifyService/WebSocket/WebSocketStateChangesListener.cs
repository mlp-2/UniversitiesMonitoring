using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.WebSocket;

internal class WebSocketStateChangesListener : IStateChangesListener, IDisposable
{
    private readonly ILogger<WebSocketStateChangesListener> _logger;
    private const int BufferSize = 1 << 9;

    private readonly Uri _wsRoute;
    private ClientWebSocket _wsClient = new();

    public WebSocketStateChangesListener(IConfiguration configuration, ILogger<WebSocketStateChangesListener> logger)
    {
        _logger = logger;
        var localhostUrl = Environment.GetEnvironmentVariable("WS_URL") ??
                           configuration["LocalHostWsUrl"];
        _wsRoute = new Uri(localhostUrl);
    }

    public async Task ConnectAsync()
    {
        var connected = false;

        while (!connected)
        {
            try
            {
                await _wsClient.ConnectAsync(_wsRoute, CancellationToken.None);
                connected = _wsClient.State == WebSocketState.Open;
            }
            catch
            {
                _logger.LogWarning("Can't connect to WS. Retry in 10 seconds");
                _wsClient = new ClientWebSocket();
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        _logger.LogInformation("Connected to WS");
    }

    private async Task ReconnectAsync()
    {
        _wsClient = new ClientWebSocket();
        await ConnectAsync();
    }

    /// <summary>
    /// Получает все изменения состояний полученные по WS
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены операции</param>
    /// <returns>Коллекция изменений состояний</returns>
    /// <exception cref="InvalidOperationException">Вызывается, если клиент не подключен к сокету или передан неверный тип WS сообщения</exception>
    public async Task<IEnumerable<UniversityServiceChangeStateEntity>> TryGetChangesAsync(
        CancellationToken cancellationToken)
    {
        if (_wsClient.State != WebSocketState.Open)
        {
            await ReconnectAsync();
        }

        try
        {
            var buffer = new byte[BufferSize];
            await using var outputStream = new MemoryStream(BufferSize);
            var receiveResult = await _wsClient.ReceiveAsync(buffer, cancellationToken);

            if (receiveResult.MessageType != WebSocketMessageType.Text)
            {
                throw new InvalidOperationException("Expected text WS message type");
            }

            await outputStream.WriteAsync(buffer, 0, receiveResult.Count, cancellationToken);

            return JsonSerializer.Deserialize<UniversityServiceChangeStateEntity[]>(
                Encoding.UTF8.GetString(outputStream.ToArray())) ?? Array.Empty<UniversityServiceChangeStateEntity>();
        }
        catch
        {
            await ReconnectAsync();
            return await TryGetChangesAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        _wsClient.Dispose();
    }
}