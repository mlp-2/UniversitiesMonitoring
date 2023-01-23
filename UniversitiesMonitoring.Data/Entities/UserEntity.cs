using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class UserEntity
{
    public UserEntity(User userModel)
    {
        Id = userModel.Id;
        Email = userModel.SendEmailNotification ? userModel.Email ?? null : null;
        TelegramTag = userModel.SendTelegramNotification ? userModel.TelegramTag ?? null : null;
    }

    [JsonConstructor]
    public UserEntity(ulong id, string? email, string? telegramTag)
    {
        Id = id;
        Email = email;
        TelegramTag = telegramTag;
    }
    
    public ulong Id { get; }
    public string? Email { get; }
    public string? TelegramTag { get; }
}