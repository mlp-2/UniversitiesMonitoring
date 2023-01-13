using System.Text.Json.Serialization;

namespace UniversitiesMonitoring.Api.Entities;

public class UniversityServiceChangeStateEntity
{
    [JsonConstructor]
    public UniversityServiceChangeStateEntity(ulong id, ulong universityId, string serviceName, string universityName)
    {
        Id = id;
        UniversityId = universityId;
        ServiceName = serviceName;
        UniversityName = universityName;
    }

    public ulong Id { get; }
    public ulong UniversityId { get; }
    public string ServiceName { get; }
    public string UniversityName { get; }
}