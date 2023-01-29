using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Services;

internal interface IModeratorProvider
{
    Task<Moderator?> GetModeratorAsync(ulong moderatorId);
}