using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService.Providers;

internal interface IUniversitiesServiceProvider
{
    /// <summary>
    /// Получает все сервисы ВУЗов, которые есть
    /// </summary>
    Task<IEnumerable<UniversityServiceEntity>> GetAllUniversitiesServicesAsync();
    
    /// <summary>
    /// Отправляет изменения состояний
    /// </summary>
    /// <param name="update">Сущность, описывающая изменения</param>
    Task SendUpdateAsync(UpdateEntity update);
}