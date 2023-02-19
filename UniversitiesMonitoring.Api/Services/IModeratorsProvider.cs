using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

public interface IModeratorsProvider
{
    Task AcceptReportAsync(UniversityServiceReport report);
    Task<Moderator?> GetModeratorAsync(ulong moderatorId);
}