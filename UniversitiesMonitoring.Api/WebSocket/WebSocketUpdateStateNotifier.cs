using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using UniversitiesMonitoring.Api.Services;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;
using WS = System.Net.WebSockets.WebSocket;

namespace UniversitiesMonitoring.Api.WebSocket;

public class WebSocketUpdateStateNotifier : IWebSocketUpdateStateNotifier
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<Tuple<WS, TaskCompletionSource<object>>> _webSockets = new();

    public WebSocketUpdateStateNotifier(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void AppendWebSocket(WS webSocket, TaskCompletionSource<object> socketFinishedTcs)
    {
        _webSockets.Add(new Tuple<WS, TaskCompletionSource<object>>(webSocket, socketFinishedTcs));
    }

    public async Task NotifyAsync(ulong[] servicesIds)
    {
        await using var changesDataStream = new MemoryStream();
        var badSockets = new List<Tuple<WS, TaskCompletionSource<object>>>();
        
        var changes = await CreateChangeStatesReportsAsync(servicesIds);
        var changesJsonBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(changes));

        foreach (var webSocketTup in _webSockets)
        {
            var webSocket = webSocketTup.Item1;
            var tcs = webSocketTup.Item2;
            
            if (webSocket.CloseStatus.HasValue)
            {
                badSockets.Add(webSocketTup);
                tcs.SetResult(new object());
            }

            await webSocket.SendAsync(changesJsonBytes, WebSocketMessageType.Text, 
                                        WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
        }

        _webSockets.RemoveAll(x => badSockets.Contains(x));
    }

    private async Task<UniversityServiceChangeStateEntity[]> CreateChangeStatesReportsAsync(ulong[] servicesIds)
    {
        var changes = new UniversityServiceChangeStateEntity[servicesIds.Length];
        using var scope = _serviceProvider.CreateScope();
        var universitiesServices = scope.ServiceProvider.GetRequiredService<IDataProvider>().UniversityServices;
        
        for (var serviceIndex = 0; serviceIndex < servicesIds.Length; serviceIndex++)
        {
            var serviceId = servicesIds[serviceIndex];
            var service = await universitiesServices.FindAsync(serviceId);

            if (service == null)
            {
                throw new InvalidOperationException($"Service with {service} hasn't found");
            }

            var serviceStatus = service.UniversityServiceStateChanges.OrderByDescending(x => x.ChangedAt).FirstOrDefault()?.IsOnline ?? false;
            changes[serviceIndex] = new UniversityServiceChangeStateEntity(serviceId, service.UniversityId, service.Name, service.University.Name, serviceStatus);
        }

        return changes;
    }
}