using System.Net.Http.Json;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService.Helpers;

internal class ServicesFinder
{
    private readonly HttpClient _client;
    
    public ServicesFinder(IConfiguration configuration)
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_URL") ??
                                  configuration["ApiUrl"])
        };
    }

    public async Task<IEnumerable<UniversityServiceEntity>> GetServicesEntityAsync(IEnumerable<ulong> servicesIds)
    {
        var servicesIdsQuery = "";
        foreach (var id in servicesIds) servicesIdsQuery += $"ids={id}*";

        var response = await _client.GetAsync("/api/services?loadUsers=true&" + servicesIdsQuery);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UniversityServiceEntity[]>() ?? Array.Empty<UniversityServiceEntity>();
    }
}