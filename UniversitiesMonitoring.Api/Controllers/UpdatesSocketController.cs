using UniversitiesMonitoring.Api.WebSocket;

namespace UniversitiesMonitoring.Api.Controllers;

internal class UpdatesSocketController : ControllerBase
{
    private readonly IWebSocketUpdateStateNotifier _webSocketUpdateStateNotifier;

    public UpdatesSocketController(IWebSocketUpdateStateNotifier webSocketUpdateStateNotifier)
    {
        _webSocketUpdateStateNotifier = webSocketUpdateStateNotifier;
    }
    
    [Route("/updates-socket")]
    public async Task<IActionResult> UpdatesSocket()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            var socketFinishedTcs = new TaskCompletionSource<object>();
            _webSocketUpdateStateNotifier.AppendWebSocket(webSocket, socketFinishedTcs);

            await socketFinishedTcs.Task;
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}