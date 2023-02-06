using UniversitiesMonitoring.Api.Entities;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

internal interface IServicesProvider
{
    Task<UniversityService?> GetServiceAsync(ulong serviceId);
    IEnumerable<UniversityService> GetAllServices();
    Task SubscribeUserAsync(User user, UniversityService service);
    Task UnsubscribeUserAsync(User user, UniversityService service);
    Task UpdateServiceStateAsync(UniversityService service, bool isOnline, bool forceSafe);
    Task LeaveCommentAsync(UniversityService service, User author, Comment comment);        
    Task CreateReportAsync(UniversityService service, User issuer, Report report);        
    Task<UniversityServiceReport?> GetReportAsync(ulong reportId);
    IEnumerable<UniversityServiceReport> GetAllReports();
    Task DeleteReportAsync(UniversityServiceReport report);
}