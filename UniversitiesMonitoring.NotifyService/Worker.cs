using Microsoft.Extensions.Caching.Memory;
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
    private readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));
    
    public Worker(ILogger<Worker> logger,
        IStateChangesListener stateChangesListener,
        IServiceProvider serviceProvider,
        ServicesFinder servicesFinder)
    {
        _logger = logger;
        _stateChangesListener = stateChangesListener;
        _serviceProvider = serviceProvider;
        _servicesFinder = servicesFinder;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _stateChangesListener.ConnectAsync();
        }
        catch
        {
            _logger.LogCritical("Can't connect to WS");
            return;
        }
        
        using var scope = _serviceProvider.CreateScope();

        while (true)
        {
            var dataFromSocket = await _stateChangesListener.TryGetChangesAsync(stoppingToken);
            var dataFromSocketInArray = dataFromSocket as UniversityServiceChangeStateEntity[] ?? dataFromSocket.ToArray();
            if (!dataFromSocketInArray.Any()) continue;

            var services = await _servicesFinder.GetServicesEntityAsync(from update in dataFromSocketInArray select update.Id);
            
            foreach (var service in services)
            {
                foreach (var serviceSubscriber in service.Subscribers)
                {
                    var notifyContext = new NotifyContext(serviceSubscriber, scope.ServiceProvider);
                    await notifyContext.NotifyAsync(service);
                }
            }
        }
    }
}