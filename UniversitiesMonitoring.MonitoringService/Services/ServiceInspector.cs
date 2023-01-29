using System.Net;
using UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

namespace UniversitiesMonitoring.MonitoringService.Services;

internal class ServiceInspector : IServicesInspector
{
    private IEnumerable<IInspectingStrategy> _inspectingStrategies;
    
    public ServiceInspector(IEnumerable<IInspectingStrategy> inspectingStrategies)
    {
        _inspectingStrategies = inspectingStrategies;
    }
    
    public async Task<bool> InspectServiceAsync(IPAddress ip)
    {
        foreach (var inspectingStrategy in _inspectingStrategies)
        {
            if (await inspectingStrategy.ExecuteStrategyAsync(ip)) return true;
        }

        return false;
    }
}