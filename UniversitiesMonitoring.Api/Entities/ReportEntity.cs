using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Entities;

public class ReportEntity
{
    public ReportEntity(UniversityServiceReport report)
    {
        Id = report.Id;
        Content = report.Content;
        Timestamp = report.AddedAt;
        IsOnline = report.IsOnline;
        Service = new UniversityServiceEntity(report.Service, false, false);
    }
    
    public ulong Id { get; }
    public string? Content { get; }
    public DateTime? Timestamp { get; }
    public bool IsOnline { get; }
    public UniversityServiceEntity Service { get; }
}