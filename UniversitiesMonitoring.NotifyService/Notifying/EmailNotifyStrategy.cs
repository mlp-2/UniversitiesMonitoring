using System.Net;
using System.Net.Mail;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

internal class EmailNotifyStrategy : INotifyStrategy
{
    private readonly SmtpClient _emailClient;
    private readonly MailAddress _mailAddress;
    
    public EmailNotifyStrategy(IConfiguration configuration)
    {
        var address = configuration["Email:Username"];
        _mailAddress = new MailAddress(address);
        _emailClient = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            Credentials = new NetworkCredential(address, configuration["Email:Password"]),
            EnableSsl = true
        };
    }
    
    public async Task NotifyAsync(UserEntity userEntity, UniversityServiceEntity serviceEntity)
    {
        if (userEntity.Email == null) return;
        var message = CreateMailMessage(serviceEntity.ServiceName, serviceEntity.IsOnline);
        message.To.Add(userEntity.Email);
        
        await _emailClient.SendMailAsync(message);
    }

    private MailMessage CreateMailMessage(string serviceName, bool isOnline) =>
        new()
        {
            From = _mailAddress,
            Subject = "Изменение состояния сервиса",
            Body =
                $"<b>📢 Сервис {serviceName} изменил свое состояние на {(isOnline ? "онлайн 🟢" : "офлайн 🔴")}</b>\n" +
                "Данное сообщение отправлено, т.к. Вы подписались на обновления данного сервиса. Если Вам не нужны уведомления о нём Вы можете от него отписаться",
            IsBodyHtml = true
        };
}