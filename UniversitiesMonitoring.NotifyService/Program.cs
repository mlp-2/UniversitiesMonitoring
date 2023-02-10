using UniversitiesMonitoring.NotifyService;
using UniversitiesMonitoring.NotifyService.Helpers;
using UniversitiesMonitoring.NotifyService.Notifying;
using UniversitiesMonitoring.NotifyService.WebSocket;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ServicesFinder>()
            .AddSingleton<EmailNotifier>()
            .AddSingleton<IStateChangesListener, WebSocketStateChangesListener>()
            .AddHostedService<Worker>();
    })
    .Build();

host.Run();