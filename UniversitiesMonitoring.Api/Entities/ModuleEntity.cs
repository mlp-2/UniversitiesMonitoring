namespace UniversitiesMonitoring.Api.Entities;

public class ModuleEntity
{
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