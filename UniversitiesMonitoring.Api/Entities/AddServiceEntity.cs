using Newtonsoft.Json;

namespace UniversitiesMonitoring.Api.Entities;

public class AddServiceEntity
{
    [JsonConstructor]
    public AddServiceEntity(string name, string url, ulong universityId)
    {
        Name = name;
        Url = url;
        UniversityId = universityId;
    }

    public string Name { get; }
    public string Url { get; }
    public ulong UniversityId { get; }
}