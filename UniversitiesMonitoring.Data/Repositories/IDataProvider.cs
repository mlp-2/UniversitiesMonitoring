using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Repositories;

public interface IDataProvider : IDisposable
{
    IRepository<University, ulong> Universities { get; }
    IRepository<UniversityService, ulong> UniversityServices { get; }
    IRepository<UniversityServiceStateChange, ulong> UniversityServiceStateChange { get; }
    IRepository<UserSubscribeToService, ulong> Subscribes { get; }
    IRepository<User, ulong> Users { get; }
    IRepository<Moderator, ulong> Moderators { get; }
    IRepository<UniversityServiceReport, ulong> Reports { get; }
    IRepository<UserRateOfService, ulong> Rates { get; }

    /// <summary>
    /// Сохраняет все изменения
    /// </summary>
    Task SaveChangesAsync();
}