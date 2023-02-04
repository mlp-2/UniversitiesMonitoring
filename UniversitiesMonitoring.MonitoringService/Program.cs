using UniversitiesMonitoring.MonitoringService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => services.AddHostedService<Worker>())
    .Build();

host.Run();