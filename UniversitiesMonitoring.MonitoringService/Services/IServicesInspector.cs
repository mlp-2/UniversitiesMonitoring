using System.Net;

namespace UniversitiesMonitoring.MonitoringService.Services;

internal interface IServicesInspector
{
    Task<bool> InspectServiceAsync(IPAddress ip);
}
