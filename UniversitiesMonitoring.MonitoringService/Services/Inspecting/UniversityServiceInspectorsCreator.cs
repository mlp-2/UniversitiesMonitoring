using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal static class UniversityServiceInspectorsCreator
{
    public static IEnumerable<UniversityServiceInspector> CreateInspectors(IServiceInspector inspector,
        IEnumerable<UniversityServiceEntity> services) =>
        from service in services select new UniversityServiceInspector(inspector, service);
}