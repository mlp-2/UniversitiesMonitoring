using System.Text.Json.Serialization;

namespace UniversitiesMonitoring.Api.Entities;

public class ModeratorAuthEntity
{
    [JsonConstructor]
    public ModeratorAuthEntity(ulong id, string password)
    {
        Id = id;
        Password = password;
    }

    public ulong Id { get; }
    public string Password { get; } 
}