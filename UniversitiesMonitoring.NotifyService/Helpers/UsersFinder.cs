using System.Net.Http.Json;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Helpers;

public class UsersFinder
{
    private readonly HttpClient _client;
    
    public UsersFinder(IConfiguration configuration)
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:" + configuration["ApiPort"])
        };
    }

    public async Task<IEnumerable<UniversityServiceEntity>> GetServicesEntity(IEnumerable<ulong> servicesIds)
    {
        var servicesIdsQuery = "";
        foreach (var id in servicesIds) servicesIdsQuery += $"ids={id}*";

        var response = await _client.GetAsync("/services?loadUsers=true&" + servicesIdsQuery);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UniversityServiceEntity[]>() ?? Array.Empty<UniversityServiceEntity>();
    }
}