using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class UserEntity
{
    public UserEntity(User userModel)
    {
        Id = userModel.Id;
        Username = userModel.Username;
        Email = userModel.SendEmailNotification ? userModel.Email ?? null : null;
        TelegramTag = userModel.SendTelegramNotification ? userModel.TelegramTag ?? null : null;
    }

    [JsonConstructor]
    public UserEntity(ulong id, string username, string? email, string? telegramTag)
    {
        Id = id;
        Username = username;
        Email = email;
        TelegramTag = telegramTag;
    }
    
    public ulong Id { get; }
    public string Username { get; }
    public string? Email { get; }
    public string? TelegramTag { get; }
}