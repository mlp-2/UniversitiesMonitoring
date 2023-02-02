using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

internal class EverywhereNotifyStrategy : INotifyStrategy
{
    private readonly TelegramNotifyStrategy _telegramStrategy;
    private readonly EmailNotifyStrategy _emailStrategy;

    public EverywhereNotifyStrategy(TelegramNotifyStrategy telegramStrategy, EmailNotifyStrategy emailStrategy)
    {
        _telegramStrategy = telegramStrategy;
        _emailStrategy = emailStrategy;
    }

    public Task NotifyAsync(UserEntity userEntity, UniversityServiceEntity serviceEntity) => Task.WhenAll(
        _telegramStrategy.NotifyAsync(userEntity, serviceEntity),
        _emailStrategy.NotifyAsync(userEntity, serviceEntity));
}