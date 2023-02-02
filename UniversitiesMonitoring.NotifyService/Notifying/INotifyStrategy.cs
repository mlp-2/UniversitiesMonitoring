using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

internal interface INotifyStrategy
{
    /// <summary>
    /// Оповещает пользователя
    /// </summary>
    /// <param name="userEntity">Данные, по которым происходит оповещение</param>
    /// <param name="serviceEntity">Сервис, о котором идет оповещение</param>
    Task NotifyAsync(UserEntity userEntity, UniversityServiceEntity serviceEntity);
}