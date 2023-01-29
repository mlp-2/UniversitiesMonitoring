using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.MonitoringService.Services.Inspecting;

internal static class UniversityServiceInspectorsCreator
{
    public static IEnumerable<UniversityServiceInspector> CreateInspectorsAsync(IServicesInspector inspector, IEnumerable<UniversityServiceEntity> services)
    {
        foreach (var service in services)
        {
            yield return new UniversityServiceInspector(inspector, service);
        }
    }
}