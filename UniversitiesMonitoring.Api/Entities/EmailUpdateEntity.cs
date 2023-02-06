using System.Text.Json.Serialization;

namespace UniversitiesMonitoring.Api.Entities;

internal class EmailUpdateEntity
{
    [JsonConstructor]
    public EmailUpdateEntity(string email, bool canSend)
    {
        Email = email;
        CanSend = canSend;
    }

    public string Email { get; }
    public bool CanSend { get; }
}