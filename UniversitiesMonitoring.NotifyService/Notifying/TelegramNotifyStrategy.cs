using Telegram.Bot;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

internal class TelegramNotifyStrategy : INotifyStrategy
{
    private readonly TelegramBotClient _tgClient;

    public TelegramNotifyStrategy(string telegramApiToken)
    {
        _tgClient = new TelegramBotClient(telegramApiToken);
    }
    
    public async Task NotifyAsync(UserEntity userEntity, UniversityServiceEntity serviceEntity)
    {
        if (userEntity.TelegramTag == null) return;

        await _tgClient.SendTextMessageAsync(userEntity.TelegramTag,
            CreateNotifyMessage(serviceEntity.ServiceName, serviceEntity.IsOnline));
    }

    private string CreateNotifyMessage(string serviceName, bool isOnline) =>
        $"***📢 Сервис {serviceName} изменил свое состояние на {(isOnline ? "онлайн 🟢" : "офлайн 🔴")}***\n" +
        "Данное сообщение отправлено, т.к. Вы подписались на обновления данного сервиса. Если Вам не нужны уведомления о нём Вы можете от него отписаться";
}