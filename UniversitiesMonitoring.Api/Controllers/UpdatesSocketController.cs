using UniversitiesMonitoring.Api.WebSocket;

namespace UniversitiesMonitoring.Api.Controllers;

[ApiController]
public class UpdatesSocketController : ControllerBase
{
    private readonly IWebSocketUpdateStateNotifier _webSocketUpdateStateNotifier;

    public UpdatesSocketController(IWebSocketUpdateStateNotifier webSocketUpdateStateNotifier)
    {
        _webSocketUpdateStateNotifier = webSocketUpdateStateNotifier;
    }
    
    [HttpGet]
    [Route("/api/updates-socket")]
    public async Task UpdatesSocket()
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {              
            HttpContext.Response.StatusCode = 400;
            return;    
        }
        
        using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        var socketFinishedTcs = new TaskCompletionSource<object>();
        _webSocketUpdateStateNotifier.AppendWebSocket(webSocket, socketFinishedTcs);

        await socketFinishedTcs.Task;
    }
}