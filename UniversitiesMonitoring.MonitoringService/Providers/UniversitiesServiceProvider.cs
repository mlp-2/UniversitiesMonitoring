using System.Net.Http.Json;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService.Providers;

internal class UniversitiesServiceProvider : IUniversitiesServiceProvider
{
    private readonly ILogger _logger;
    private readonly HttpClient _client;
    
    public UniversitiesServiceProvider(IConfiguration configuration, ILogger<UniversitiesServiceProvider> logger)
    {
        _logger = logger;
        _client = new HttpClient()
        {
            BaseAddress = new Uri(Environment.GetEnvironmentVariable("API_URL") ??
                                  configuration["ApiUrl"])
        };
    }
    
    public async Task<IEnumerable<UniversityServiceEntity>> GetAllUniversitiesServicesAsync()
    {
        var response = await _client.GetAsync("/api/services");

        response.EnsureSuccessStatusCode();

        _logger.LogTrace("Got all universities. Status: {HttpStatus}", response.StatusCode);
        return await response.Content.ReadFromJsonAsync<UniversityServiceEntity[]>() ?? Array.Empty<UniversityServiceEntity>();
    }
    
    public async Task SendUpdateAsync(ChangeStateEntity[] update)
    {
        var response = await _client.PutAsJsonAsync("/api/services/update", update);
        response.EnsureSuccessStatusCode();
        
        _logger.LogTrace("Changes sent. Status: {HttpStatus}", response.StatusCode);
    }
}