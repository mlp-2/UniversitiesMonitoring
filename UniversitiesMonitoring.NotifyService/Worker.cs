using UniversitiesMonitoring.NotifyService.Helpers;
using UniversitiesMonitoring.NotifyService.Notifying;
using UniversitiesMonitoring.NotifyService.WebSocket;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.NotifyService;

internal class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IStateChangesListener _stateChangesListener;
    private readonly ServicesFinder _servicesFinder;
    private readonly EmailNotifier _emailNotifier;
    private readonly GlobalTelegramNotifying _telegramNotifying;

    public Worker(ILogger<Worker> logger,
        IStateChangesListener stateChangesListener,
        ServicesFinder servicesFinder,
        EmailNotifier emailNotifier,
        GlobalTelegramNotifying telegramNotifying)
    {
        _logger = logger;
        _stateChangesListener = stateChangesListener;
        _servicesFinder = servicesFinder;
        _emailNotifier = emailNotifier;
        _telegramNotifying = telegramNotifying;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _stateChangesListener.ConnectAsync();

        while (true)
        {
            var dataFromSocket = await _stateChangesListener.TryGetChangesAsync(stoppingToken);
            var dataFromSocketInArray = dataFromSocket as UniversityServiceChangeStateEntity[] ?? dataFromSocket.ToArray();
            if (!dataFromSocketInArray.Any()) continue;

            var services = (await _servicesFinder.GetServicesEntityAsync(from update in dataFromSocketInArray select update.Id)).ToArray();
            var countSkipped = 0;
            
            _logger.LogInformation("New update information Got");

            await _telegramNotifying.NotifyAsync(services);
            
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