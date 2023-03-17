namespace UniversitiesMonitoring.MonitoringService.Services;

internal interface IServiceInspector
{
    Task<bool> InspectServiceAsync(Uri serviceUrl);
}