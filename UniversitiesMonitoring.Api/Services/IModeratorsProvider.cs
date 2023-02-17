using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

public interface IModeratorsProvider
{
    Task<Moderator?> GetModeratorAsync(ulong moderatorId);
}