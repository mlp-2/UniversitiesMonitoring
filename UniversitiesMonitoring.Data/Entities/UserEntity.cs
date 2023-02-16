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
        SendToEmail = userModel.SendEmailNotification;
    }

    [JsonConstructor]
    public UserEntity(ulong id, string username, string? email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
    
    [JsonPropertyName("id")]
    public ulong Id { get; }
    
    [JsonPropertyName("username")]
    public string Username { get; }
    
    [JsonPropertyName("email")]
    public string? Email { get; }
    
    [JsonPropertyName("sendToEmail")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SendToEmail { get; }
}