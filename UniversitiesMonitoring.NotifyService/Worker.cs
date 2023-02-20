using UniversitiesMonitoring.NotifyService.Helpers;
using UniversitiesMonitoring.NotifyService.Notifying;
using UniversitiesMonitoring.NotifyService.WebSocket;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IStateChangesListener _stateChangesListener;
    private readonly IServiceProvider _serviceProvider;
    private readonly ServicesFinder _servicesFinder;
    private readonly EmailNotifier _emailNotifier;
    
    public Worker(ILogger<Worker> logger,
        IStateChangesListener stateChangesListener,
        IServiceProvider serviceProvider,
        ServicesFinder servicesFinder,
        EmailNotifier emailNotifier)
    {
        _logger = logger;
        _stateChangesListener = stateChangesListener;
        _serviceProvider = serviceProvider;
        _servicesFinder = servicesFinder;
        _emailNotifier = emailNotifier;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connected = false;

        while (!connected)
        {
            try
            {
                await _stateChangesListener.ConnectAsync();
                connected = true;
                _logger.LogInformation("Connected to WS");
            }
            catch
            {
                _logger.LogWarning("Can't connect to the WS. Retry in 10 seconds");
                await Task.Delay(10000, stoppingToken);
            }
        }
        
        using var scope = _serviceProvider.CreateScope();

        while (true)
        {
            var dataFromSocket = await _stateChangesListener.TryGetChangesAsync(stoppingToken);
            var dataFromSocketInArray = dataFromSocket as UniversityServiceChangeStateEntity[] ?? dataFromSocket.ToArray();
            if (!dataFromSocketInArray.Any()) continue;

            var services = await _servicesFinder.GetServicesEntityAsync(from update in dataFromSocketInArray select update.Id);
            var countSkipped = 0;
            
            _logger.LogInformation("New update information Got");
            
            foreach (var service in services)
            {
                foreach (var serviceSubscriber in service.Subscribers)
                {
                    try
                    {
                        await _emailNotifier.NotifyAsync(serviceSubscriber, service);
                    }
                    catch
                    {
                        countSkipped += 1;
                    }
                }
            }

            if (countSkipped > 0)
            {
                _logger.LogWarning("Skipped {CountSkipped} users", countSkipped);
            }
        }
    }
}