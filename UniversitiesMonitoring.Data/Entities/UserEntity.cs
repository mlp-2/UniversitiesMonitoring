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
    }

    [JsonConstructor]
    public UserEntity(ulong id, string username, string? email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
    
    public ulong Id { get; }
    public string Username { get; }
    public string? Email { get; }
}