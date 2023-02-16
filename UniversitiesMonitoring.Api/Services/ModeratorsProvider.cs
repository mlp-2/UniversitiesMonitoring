using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Services;

internal class ModeratorsProvider : IModeratorsProvider
{
    private readonly IDataProvider _dataProvider; 
    
    public ModeratorsProvider(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }
    
    public async Task<Moderator?> GetModeratorAsync(ulong moderatorId)
    {
        return await _dataProvider.Moderators.FindAsync(moderatorId);
    }
}