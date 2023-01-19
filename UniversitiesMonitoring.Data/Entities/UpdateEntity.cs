using System.Text.Json.Serialization;

namespace UniversityMonitoring.Data.Entities;

public class UpdateEntity
{
    public UpdateEntity(ChangeStateEntity[] changes)
    {
        Changes = changes;
    }
    
    [JsonPropertyName("changes")]
    public ChangeStateEntity[] Changes { get; }
}