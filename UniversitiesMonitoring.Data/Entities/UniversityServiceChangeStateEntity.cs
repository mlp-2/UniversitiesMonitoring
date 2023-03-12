using System.Text.Json.Serialization;

namespace UniversityMonitoring.Data.Entities;

public class UniversityServiceChangeStateEntity
{
    [JsonConstructor]
    public UniversityServiceChangeStateEntity(ulong id,
        ulong universityId,
        string serviceName,
        string universityName,
        bool isOnline)
    {
        Id = id;
        UniversityId = universityId;
        ServiceName = serviceName;
        UniversityName = universityName;
        IsOnline = isOnline;
    }

    [JsonPropertyName("id")] public ulong Id { get; }

    [JsonPropertyName("universityId")] public ulong UniversityId { get; }

    [JsonPropertyName("serviceName")] public string ServiceName { get; }

    [JsonPropertyName("universityName")] public string UniversityName { get; }

    [JsonPropertyName("isOnline")] public bool IsOnline { get; }
}