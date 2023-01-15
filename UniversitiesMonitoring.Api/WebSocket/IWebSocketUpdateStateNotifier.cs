using WS = System.Net.WebSockets.WebSocket;

namespace UniversitiesMonitoring.Api.WebSocket;

internal interface IWebSocketUpdateStateNotifier
{
    /// <summary>
    /// Добавляет WS к прослушиванию обновлений
    /// </summary>
    /// <param name="webSocket">Вебсокет</param>
    /// <param name="socketFinishedTcs">Обеспечение завершения работы вебсокета</param>
    void AppendWebSocket(WS webSocket, TaskCompletionSource<object> socketFinishedTcs);
    
    /// <summary>
    /// Оповещает всех слушателей о том, что все указанные сервисы изменили свое состояние
    /// </summary>
    /// <param name="servicesIds">ID сервисов, которые изменили свое состояние</param>
    Task NotifyAsync(ulong[] servicesIds);
}