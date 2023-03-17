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

    public async Task<IEnumerable<UniversityServiceEntity>> GetAllUniversitiesServicesAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.GetAsync("/api/services", cancellationToken);

            response.EnsureSuccessStatusCode();

            _logger.LogTrace("Got all universities. Status: {HttpStatus}", response.StatusCode);
            return await response.Content.ReadFromJsonAsync<UniversityServiceEntity[]>(
                cancellationToken: cancellationToken) ?? Array.Empty<UniversityServiceEntity>();
        }
        catch (Exception e)
        {
            await WaitUntilApiUnavailable(cancellationToken);
            _logger.LogError(e.ToString());
            return await GetAllUniversitiesServicesAsync(cancellationToken);
        }
    }

    public async Task SendUpdateAsync(ChangeStateEntity[] update, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.PutAsJsonAsync("/api/services/update", update, cancellationToken);
            response.EnsureSuccessStatusCode();

            _logger.LogTrace("Changes sent. Status: {HttpStatus}", response.StatusCode);
        }
        catch (Exception e)
        {
            await WaitUntilApiUnavailable(cancellationToken);
            _logger.LogError(e.ToString());
            await SendUpdateAsync(update, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task SendStatsAsync(ServiceStatisticsEntity[] stats, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("/api/services/send-statistics", stats, cancellationToken);
            response.EnsureSuccessStatusCode();

            _logger.LogTrace("Stats sent. Status: {HttpStatus}", response.StatusCode);
        }
        catch (Exception e)
        {
            await WaitUntilApiUnavailable(cancellationToken);
            _logger.LogError(e.ToString());
            await SendStatsAsync(stats, cancellationToken);
        }
    }

    private async Task WaitUntilApiUnavailable(CancellationToken cancellationToken)
    {
        _logger.LogWarning("API is unavailable. Waiting changing state of API");
        var apiIsUnavailable = true;

        while (apiIsUnavailable && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, string.Empty),
                    cancellationToken);

                apiIsUnavailable = !response.IsSuccessStatusCode;
            }
            catch
            {
                // ignored
            }

            await Task.Delay(100, cancellationToken);
        }

        if (cancellationToken.IsCancellationRequested)
        {
            throw new InvalidOperationException("Cancellation requested due waiting API");
        }

        _logger.LogInformation("API is up. Monitoring continued");
    }
}