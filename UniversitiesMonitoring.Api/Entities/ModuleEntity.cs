using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Entities;

public class ModuleEntity
{
    public ModuleEntity(MonitoringModule moduleModel, string locationName) 
        : this(moduleModel.Id, locationName, moduleModel.Url)
    {
        
    }
    
    public ModuleEntity(ulong id, string? locationName, string url)
    {
        Id = id;
        LocationName = locationName;
        Url = url;
    }

    public ulong Id { get; }
    public string? LocationName { get; }
    public string Url { get; }
}