using UniversitiesMonitoring.Api.Entities;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Services;

public class ServicesProvider : IServicesProvider
{
    private readonly IDataProvider _dataProvider;

    public ServicesProvider(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public Task<UniversityService?> GetServiceAsync(ulong serviceId) =>
        _dataProvider.UniversityServices.FindAsync(serviceId);

    public async Task SubscribeUserAsync(User user, UniversityService service)
    {
        var subscribe = new UserSubscribeToService()
        {
            User = user,
            Service = service
        };

        await _dataProvider.Subscribes.AddAsync(subscribe);
        await SaveChangesAsync();
    }

    public async Task UnsubscribeUserAsync(User user, UniversityService service)
    {
        var subscribe = _dataProvider.Subscribes.ExecuteSql(
            $"SELECT * FROM universities_monitoring.UserSubscribeToService WHERE UserId={user.Id} AND ServiceId={service.Id}").FirstOrDefault();

        if (subscribe == null)
        {
            throw new InvalidOperationException($"Can't find user({user.Id})'s subscribe to the service({service.Id})");
        }
        
        _dataProvider.Subscribes.Remove(subscribe);
        await SaveChangesAsync();
    }

    public async Task UpdateServiceStateAsync(UniversityService service, bool isOnline, bool forceSafe)
    {
        var updateState = new UniversityServiceStateChange()
        {
            Service = service,
            IsOnline = isOnline
        };
        
        await _dataProvider.UniversityServiceStateChange.AddAsync(updateState);

        if (forceSafe) await SaveChangesAsync();
    }

    public async Task LeaveCommentAsync(UniversityService service, User author, Comment comment)
    {
        var rate = new UserRateOfService()
        {
            Rate = comment.Rate,
            Comment = comment.Content
        };

        await _dataProvider.Rates.AddAsync(rate);
        await SaveChangesAsync();
    }

    public async Task CreateReportAsync(UniversityService service, User issuer, Report report)
    {
        var reportEntity = new UniversityServiceReport()
        {
            Content = report.Content,
            IsOnline = report.IsOnline,
            Issuer = issuer,
            Service = service
        };

        await _dataProvider.Reports.AddAsync(reportEntity);
        await SaveChangesAsync();
    }

    public Task<UniversityServiceReport?> GetReportAsync(ulong reportId) => _dataProvider.Reports.FindAsync(reportId);

    public async Task<IEnumerable<UniversityServiceReport>?> GetAllReportsAsync(ulong serviceId)
    {
        var service = await _dataProvider.UniversityServices.FindAsync(serviceId);

        if (service == null) return null;

        return service.UniversityServiceReports;
    }

    public Task DeleteReportAsync(UniversityServiceReport report)
    {
        _dataProvider.Reports.Remove(report);
        return SaveChangesAsync();
    }
    
    private async Task SaveChangesAsync() => await _dataProvider.SaveChangesAsync();
}