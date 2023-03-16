using System.Text.Json.Serialization;

namespace UniversityMonitoring.Data.Entities;

public class ChangeStateEntity
{
    [JsonConstructor]
    public ChangeStateEntity(ulong serviceId, bool isOnline, long? responseTime)
    {
        ServiceId = serviceId;
        IsOnline = isOnline;
        ResponseTime = responseTime;
    }

    [JsonPropertyName("serviceId")] public ulong ServiceId { get; }

    [JsonPropertyName("isOnline")] public bool IsOnline { get; }
    
    [JsonPropertyName("responseTime")] public long? ResponseTime { get; }
}