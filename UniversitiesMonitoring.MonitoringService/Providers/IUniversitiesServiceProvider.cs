using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.MonitoringService.Providers;

internal interface IUniversitiesServiceProvider
{
    Task<IEnumerable<UniversityService>> GetAllUniversitiesServicesAsync();
    Task SendUpdateAsync(UpdateEntity update);
}