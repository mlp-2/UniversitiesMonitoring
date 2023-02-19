using System.Net;
using UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

namespace UniversitiesMonitoring.MonitoringService.Services;

internal class ServiceInspector : IServiceInspector
{
    private IEnumerable<IInspectingStrategy> _inspectingStrategies;
    
    public ServiceInspector(IEnumerable<IInspectingStrategy> inspectingStrategies)
    {
        _inspectingStrategies = inspectingStrategies;
    }
    
    public async Task<bool> InspectServiceAsync(Uri serviceUrl)
    {
        foreach (var inspectingStrategy in _inspectingStrategies)
        {
            if (await inspectingStrategy.ExecuteStrategyAsync(serviceUrl)) return true;
        }

        return false;
    }
}