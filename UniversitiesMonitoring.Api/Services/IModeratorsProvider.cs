using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

internal interface IModeratorsProvider
{
    Task<Moderator?> GetModeratorAsync(ulong moderatorId);
}