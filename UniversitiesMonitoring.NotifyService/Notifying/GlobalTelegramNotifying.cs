using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

internal class GlobalTelegramNotifying
{
    private readonly TelegramBotClient _botClient;
    private readonly ChatId _globalChatId;
    
    public GlobalTelegramNotifying(IConfiguration configuration)
    {
        _botClient = new TelegramBotClient(
                Environment.GetEnvironmentVariable("TELEGRAM_TOKEN") ?? 
                configuration["TelegramToken"]);
        _globalChatId = Environment.GetEnvironmentVariable("TELEGRAM_CHAT_ID") ??
                        configuration["TelegramChatId"];
    }

    public async Task NotifyAsync(IEnumerable<UniversityServiceEntity> changedServices)
    {
        var services = changedServices.ToArray();
        var reportBuilder = new StringBuilder("⚡️📢 *Появились сервисы, которые изменили свое состояние*\n");
        
        foreach (var service in services)
        {
            reportBuilder.AppendLine(GenerateStatus(service));
        }

        var report = reportBuilder.ToString();

        await _botClient.SendTextMessageAsync(_globalChatId, report, ParseMode.Markdown);
    }

    private string GenerateStatus(UniversityServiceEntity serviceEntity) => 
        $"- Сервис [\"{serviceEntity.ServiceName}\"]({CreateServiceHref(serviceEntity)}) ВУЗа *\"{serviceEntity.UniversityName}\"* сменил свое состояние на {(serviceEntity.IsOnline ? "онлайн 🟢" : "оффлайн 🔴")}";
    
    private string CreateServiceHref(UniversityServiceEntity service) =>
        $"http://univermonitoring.gym1551.ru/service?serviceId={service.ServiceId}";
}