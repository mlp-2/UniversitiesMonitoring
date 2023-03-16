using UniversitiesMonitoring.MonitoringService.Providers;
using UniversitiesMonitoring.MonitoringService.Services;
using UniversitiesMonitoring.MonitoringService.Services.Inspecting;

namespace UniversitiesMonitoring.MonitoringService;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IUniversitiesServiceProvider _universitiesServiceProvider;
    private readonly IServiceInspector _defaultInspector;

    public Worker(ILogger<Worker> logger, IUniversitiesServiceProvider universitiesServiceProvider,
        IServiceInspector defaultInspector)
    {
        _logger = logger;
        _universitiesServiceProvider = universitiesServiceProvider;
        _defaultInspector = defaultInspector;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var inspectors = Array.Empty<UniversityServiceInspector>();

        while (!stoppingToken.IsCancellationRequested)
        {
            inspectors = await RefreshServicesInspectorsList(inspectors, stoppingToken);

            if (inspectors.Length == 0)
            {
                _logger.LogWarning("No inspectors found for monitoring. Retry to refresh list in 10 minutes");
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                continue;
            }

            var updateBuilder = new UpdateBuilder();
            var statsBuilder = new StatsBuilder();
            await Task.WhenAll(inspectors.Select(inspector => inspector.UpdateStateAsync(updateBuilder, statsBuilder)));

            var update = updateBuilder.BuildUpdate();
            var stats = statsBuilder.BuildStats();
            _logger.LogTrace("{ServiceCount} changed state", update.Changes.Length);

            if (update.Changes.Length == 0)
            {
                _logger.LogTrace("Update skipped");
                await Wait5Minutes(stoppingToken);
                continue;
            }

            await _universitiesServiceProvider.SendUpdateAsync(update.Changes, stoppingToken);
            _logger.LogTrace("Update sent");

            await _universitiesServiceProvider.SendStatsAsync(stats, stoppingToken);
            _logger.LogTrace("Stats sent");

            await Wait5Minutes(stoppingToken);
        }
    }

    private async Task<UniversityServiceInspector[]> RefreshServicesInspectorsList(
        UniversityServiceInspector[] inspectors, CancellationToken cancellationToken)
    {
        var allServices = (await _universitiesServiceProvider.GetAllUniversitiesServicesAsync(cancellationToken))
            .ToArray();
        var newInspectors = new UniversityServiceInspector[allServices.Length];
        var countAdded = allServices.Length;

        for (var i = 0; i < allServices.Length; i++)
        {
            newInspectors[i] = inspectors.FirstOrDefault(inspector =>
                               {
                                   var cond = inspector.ServiceId == allServices[i].ServiceId;
                                   if (cond) countAdded--;

                                   inspector.Service = allServices[i];
                                   return cond;
                               }) ??
                               new UniversityServiceInspector(_defaultInspector, allServices[i]);
        }

        _logger.LogTrace("Loaded new {ServicesCount} services", countAdded);

        return newInspectors;
    }

    private Task Wait5Minutes(CancellationToken token) => Task.Delay(TimeSpan.FromMinutes(5), token);
}