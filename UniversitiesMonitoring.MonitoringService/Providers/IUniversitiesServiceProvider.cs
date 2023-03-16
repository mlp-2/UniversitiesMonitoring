using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService.Providers;

internal interface IUniversitiesServiceProvider
{
    /// <summary>
    /// Получает все сервисы ВУЗов, которые есть
    /// </summary>
    Task<IEnumerable<UniversityServiceEntity>> GetAllUniversitiesServicesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Отправляет изменения состояний
    /// </summary>
    /// <param name="update">Сущность, описывающая изменения</param>
    Task SendUpdateAsync(ChangeStateEntity[] update, CancellationToken cancellationToken);

    /// <summary>
    /// Отправляет статистику
    /// </summary>
    /// <param name="stats">Статистика</param>
    Task SendStatsAsync(ServiceStatisticsEntity[] stats, CancellationToken cancellationToken);
}