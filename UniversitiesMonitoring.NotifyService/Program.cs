using UniversitiesMonitoring.NotifyService;
using UniversitiesMonitoring.NotifyService.Notifying;
using UniversitiesMonitoring.NotifyService.WebSocket;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<TelegramNotifyStrategy>()
            .AddSingleton<EmailNotifyStrategy>()
            .AddSingleton<EverywhereNotifyStrategy>()
            .AddSingleton<IStateChangesListener, WebSocketStateChangesListener>()
            .AddHostedService<Worker>();
    })
    .Build();

host.Run();