using UniversityServiceChangeState = UniversityMonitoring.Data.Entities.UniversityServiceChangeStateEntity;

namespace UniversitiesMonitoring.NotifyService.WebSocket;

internal interface IStateChangesListener
{
    /// <summary>
    /// Подключается к вебсокету для получения информации об изменении состояния сервисов
    /// </summary>
    Task ConnectAsync();
    /// <summary>
    /// Запускает асинхронную операцию по получению данных из сокета
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<UniversityServiceChangeState>> TryGetChangesAsync(CancellationToken cancellationToken);
}