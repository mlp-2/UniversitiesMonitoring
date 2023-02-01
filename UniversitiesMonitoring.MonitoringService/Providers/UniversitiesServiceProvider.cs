using System.Net.Http.Json;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService.Providers;

internal class UniversitiesServiceProvider : IUniversitiesServiceProvider
{
    private readonly HttpClient _client;
    
    public UniversitiesServiceProvider(IConfiguration configuration)
    {
        _client = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:" + configuration["ApiPort"])
        };
    }
    
    public async Task<IEnumerable<UniversityServiceEntity>> GetAllUniversitiesServicesAsync()
    {
        var response = await _client.GetAsync("/services");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UniversityServiceEntity[]>() ?? Array.Empty<UniversityServiceEntity>();
    }
    
    public async Task SendUpdateAsync(UpdateEntity update)
    {
        var response = await _client.PostAsJsonAsync("/services/update", update);
        response.EnsureSuccessStatusCode();
    }
}