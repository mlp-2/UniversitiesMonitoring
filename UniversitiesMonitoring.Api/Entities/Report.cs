using System.Text.Json.Serialization;

namespace UniversitiesMonitoring.Api.Entities;

public class Report
{
    [JsonConstructor]
    public Report(string content, bool isOnline, ulong serviceId)
    {
        Content = content;
        IsOnline = isOnline;
        ServiceId = serviceId;
    }

    public string Content { get; }
    public bool IsOnline { get; }
    public ulong ServiceId { get; }
}