using System.Text.Json.Serialization;

namespace UniversityMonitoring.Data.Entities;

public class ChangeStateEntity
{
    [JsonConstructor]
    public ChangeStateEntity(ulong serviceId, bool isOnline)
    {
        ServiceId = serviceId;
        IsOnline = isOnline;
    }
    
    [JsonPropertyName("serviceId")]
    public ulong ServiceId { get; }
    
    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; }
}