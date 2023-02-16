using System.Net;

namespace UniversitiesMonitoring.MonitoringService.Services;

internal interface IServiceInspector
{
    Task<bool> InspectServiceAsync(IPAddress ip);
}
