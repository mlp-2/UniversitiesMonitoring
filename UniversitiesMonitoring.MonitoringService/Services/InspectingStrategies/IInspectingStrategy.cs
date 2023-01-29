using System.Net;

namespace UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

public interface IInspectingStrategy
{
    Task<bool> ExecuteStrategyAsync(IPAddress ip);
}