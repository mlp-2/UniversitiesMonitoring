using UniversitiesMonitoring.MonitoringService;
using UniversitiesMonitoring.MonitoringService.Providers;
using UniversitiesMonitoring.MonitoringService.Services;
using UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => services.AddSingleton<IUniversitiesServiceProvider, UniversitiesServiceProvider>()
        .AddHostedService<Worker>()
        .AddSingleton<IEnumerable<IInspectingStrategy>, IInspectingStrategy[]>(_ => new[]
        {
            // Можно добавить еще варианты проверки
            new HeadInspectingStrategy(),
        })
        .AddSingleton<IServiceInspector, ServiceInspector>())
    .Build();

host.Run();