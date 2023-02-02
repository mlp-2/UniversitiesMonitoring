using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

public class NotifyContext
{
    private readonly UserEntity _user;
    private readonly INotifyStrategy _notifyStrategy;

    public NotifyContext(UserEntity user, IServiceProvider serviceProvider)
    {
        _user = user;
        var everywhereStrategy = serviceProvider.GetRequiredService<EverywhereNotifyStrategy>();
        var telegramStrategy = serviceProvider.GetRequiredService<TelegramNotifyStrategy>();
        var emailStrategy = serviceProvider.GetRequiredService<EmailNotifyStrategy>();

        if (user.Email != null && user.TelegramTag != null) _notifyStrategy = everywhereStrategy;
        else if (user.Email != null) _notifyStrategy = emailStrategy;
        else _notifyStrategy = telegramStrategy;
    }

    public Task NotifyAsync(UniversityServiceEntity universityService) =>
        _notifyStrategy.NotifyAsync(_user, universityService);
}