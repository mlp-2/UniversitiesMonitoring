using System.Net;
using System.Net.Mail;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Notifying;

internal class EmailNotifier
{
    private readonly SmtpClient _emailClient;
    private readonly MailAddress _mailAddress;
    
    public EmailNotifier(IConfiguration configuration)
    {
        var address = configuration["Email:Username"];
        _mailAddress = new MailAddress(address);
        _emailClient = new SmtpClient()
        {
            Host = "mail.hosting.reg.ru",
            Port = 587,
            Credentials = new NetworkCredential(address, configuration["Email:Password"]),
            EnableSsl = false
        };
    }
    
    public async Task NotifyAsync(UserEntity userEntity, UniversityServiceEntity serviceEntity)
    {
        if (userEntity.Email == null) return;
        var message = CreateMailMessage(serviceEntity.ServiceName, serviceEntity.IsOnline, serviceEntity.ServiceId);
        message.To.Add(userEntity.Email);
        
        await _emailClient.SendMailAsync(message);
    }

    private MailMessage CreateMailMessage(string serviceName, bool isOnline, ulong serviceId) =>
        new()
        {
            From = _mailAddress,
            Subject = "Изменение состояния сервиса",
            Body =
                $"<b>📢 Сервис {serviceName} изменил свое состояние на {(isOnline ? "онлайн 🟢" : "офлайн 🔴")}</b><br/>" +
                (!isOnline ? $"Чтобы узнать про возможные причины, перейдите по  <a href=\"https://universitiesmonitoring.ru/services/{serviceId}\">ссылке</a>" : string.Empty),
            IsBodyHtml = true
        };
}