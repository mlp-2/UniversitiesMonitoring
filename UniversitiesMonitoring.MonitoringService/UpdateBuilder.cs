using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService;

internal class UpdateBuilder
{
    private readonly List<ChangeStateEntity> _updates = new();

    /// <summary>
    /// Добавляет новое изменение состояния
    /// </summary>
    /// <param name="serviceId">ID сервиса</param>
    /// <param name="isOnline">true, если сервис онлайн</param>
    public void AddChangeState(ulong serviceId, bool isOnline) => 
        _updates.Add(new ChangeStateEntity(serviceId, isOnline));

    /// <summary>
    /// Создает обновление состояний
    /// </summary>
    /// <returns>Обновление состояний</returns>
    public UpdateEntity BuildUpdate() => new UpdateEntity(_updates.ToArray());
}