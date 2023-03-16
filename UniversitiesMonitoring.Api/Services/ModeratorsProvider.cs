using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Services;

internal class ModeratorsProvider : IModeratorsProvider
{
    private readonly IDataProvider _dataProvider;
    private readonly IServicesProvider _serviceProvider;

    public ModeratorsProvider(IDataProvider dataProvider, IServicesProvider servicesProvider)
    {
        _dataProvider = dataProvider;
        _serviceProvider = servicesProvider;
    }

    public async Task AcceptReportAsync(UniversityServiceReport report)
    {
        _dataProvider.Reports.GetlAll()
            .Where(x => x.AddedAt > report.AddedAt && x.IsOnline == report.IsOnline)
            .ToList()
            .ForEach(x => x.IsSolved = true);

        await _serviceProvider.UpdateServiceStateAsync(report.Service, report.IsOnline, true, null, report.AddedAt);

        await _dataProvider.SaveChangesAsync();
    }

    public async Task<Moderator?> GetModeratorAsync(ulong moderatorId)
    {
        return await _dataProvider.Moderators.FindAsync(moderatorId);
    }
}