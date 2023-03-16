using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.MonitoringService;

internal class StatsBuilder
{
    private readonly List<ServiceStatisticsEntity> _stats = new();

    public void AddStats(ulong serviceId, long responseTime) =>
        _stats.Add(new ServiceStatisticsEntity(serviceId, responseTime));

    public ServiceStatisticsEntity[] BuildStats() => _stats.ToArray();
}