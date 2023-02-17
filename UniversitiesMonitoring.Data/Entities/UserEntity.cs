using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class UserEntity
{
    public UserEntity(User userModel)
    {
        Id = userModel.Id;
        Email = userModel.SendEmailNotification ? userModel.Email ?? null : null;
    }

    public ulong Id { get; }
    public string? Email { get; }
}