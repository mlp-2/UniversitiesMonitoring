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
            BaseAddress = new Uri(configuration["ApiUrl"])
        };
    }
    
    public async Task<IEnumerable<UniversityServiceEntity>> GetAllUniversitiesServicesAsync()
    {
        var response = await _client.GetAsync("/api/services");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UniversityServiceEntity[]>() ?? Array.Empty<UniversityServiceEntity>();
    }
    
    public async Task SendUpdateAsync(ChangeStateEntity[] update)
    {
        var response = await _client.PutAsJsonAsync("/api/services/update", update);
        response.EnsureSuccessStatusCode();
    }
}