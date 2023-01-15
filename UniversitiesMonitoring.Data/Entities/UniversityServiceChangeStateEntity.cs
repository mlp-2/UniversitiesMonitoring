using System.Text.Json.Serialization;

namespace UniversityMonitoring.Data.Entities;

public class UniversityServiceChangeStateEntity
{
    [JsonConstructor]
    public UniversityServiceChangeStateEntity(ulong id, ulong universityId, string serviceName, string universityName, bool isOnline)
    {
        Id = id;
        UniversityId = universityId;
        ServiceName = serviceName;
        UniversityName = universityName;
        IsOnline = isOnline;
    }

    public ulong Id { get; }
    public ulong UniversityId { get; }
    public string ServiceName { get; }
    public string UniversityName { get; }
    public bool IsOnline { get; }
}