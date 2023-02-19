namespace UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

public interface IInspectingStrategy
{
    Task<bool> ExecuteStrategyAsync(Uri url);
}