using System.Net;

namespace UniversitiesMonitoring.MonitoringService.Services;

public interface IServicesInspector
{
    Task<bool> InspectServiceAsync(IPAddress ip);
}
