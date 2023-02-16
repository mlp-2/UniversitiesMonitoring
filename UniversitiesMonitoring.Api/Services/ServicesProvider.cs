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

    public async Task<IEnumerable<UniversityService>> GetAllServicesAsync(ulong? universityId = null)
    {
        if (universityId == null) return _dataProvider.UniversityServices.GetlAll();

        var university = await _dataProvider.Universities.FindAsync(universityId.Value);

        if (university == null) return Array.Empty<UniversityService>();

        return university.UniversityServices;
    }

    public Task<University?> GetUniversityAsync(ulong universityId) => _dataProvider.Universities.FindAsync(universityId);

    public IEnumerable<University> GetAllUniversities() => _dataProvider.Universities.GetlAll(); 

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
            Service = service,
            Author = author,
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
            Service = service,
            IsSolved = service.UniversityServiceStateChanges.LastOrDefault()?.IsOnline == report.IsOnline
        };

        await _dataProvider.Reports.AddAsync(reportEntity);
        await SaveChangesAsync();
    }

    public async Task SolveReportAsync(UniversityServiceReport report)
    {
        report.IsSolved = true;
        await SaveChangesAsync();
    }
    
    public Task<UniversityServiceReport?> GetReportAsync(ulong reportId) => _dataProvider.Reports.FindAsync(reportId);

    public IEnumerable<UniversityServiceReport> GetAllReports() => _dataProvider.Reports.GetlAll().Where(x => !x.IsSolved);

    public Task DeleteReportAsync(UniversityServiceReport report)
    {
        _dataProvider.Reports.Remove(report);
        return SaveChangesAsync();
    }

    public IEnumerable<Report> GetReportsByOffline(UniversityService service)
    {
        var lastStatus = service.UniversityServiceStateChanges.LastOrDefault();
        if (lastStatus == null || lastStatus.IsOnline) return Array.Empty<Report>();

        var lastSeenOffline = GetSqlTime(lastStatus.ChangedAt);
//        var lastSeenOnline = GetSqlTime(service.UniversityServiceStateChanges.LastOrDefault(x => x.IsOnline)?.ChangedAt ?? new DateTime(1970, 1, 1, 0, 0, 0, 0));

        return from report in  _dataProvider.Reports.ExecuteSql(
            $"SELECT * FROM universities_monitoring.UniversityServiceReport WHERE ServiceId = {service.Id} AND " +
            $"AddedAt > {lastSeenOffline}")
            select new Report(report);
    }


    private string GetSqlTime(DateTime dateTime) => 
        $"STR_TO_DATE('{dateTime.Year}-{dateTime.Month}-{dateTime.Day} {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}', '%Y-%m-%d %H:%i:%s')";
    private async Task SaveChangesAsync() => await _dataProvider.SaveChangesAsync();
}