using Microsoft.EntityFrameworkCore;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Repositories;

public class DataProvider : IDataProvider
{
    private readonly UniversitiesMonitoringContext _context;

    public DataProvider(UniversitiesMonitoringContext context)
    {
        _context = context;
        
        Universities = new Repository<University, ulong>(context);
        UniversityServices = new Repository<UniversityService, ulong>(context);
        Users = new Repository<User, ulong>(context);
        Moderators = new Repository<Moderator, ulong>(context);
        Reports = new Repository<UniversityServiceReport, ulong>(context);
    }

    public IRepository<University, ulong> Universities { get; }
    public IRepository<UniversityService, ulong> UniversityServices { get; }
    public IRepository<User, ulong> Users { get; }
    public IRepository<Moderator, ulong> Moderators { get; }
    public IRepository<UniversityServiceReport, ulong> Reports { get; }
    
    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}