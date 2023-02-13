using System.Text.Json.Serialization;

namespace UniversitiesMonitoring.Api.Entities;

public class Comment
{
    [JsonConstructor]
    public Comment(sbyte rate, string content)
    {
        Rate = rate;
        Content = content;
    }
    
    public sbyte Rate { get; }
    public string? Content { get; }
}