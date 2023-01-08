using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Repositories;

public interface IDataProvider : IDisposable
{
    IRepository<University, ulong> Universities { get; }
    IRepository<UniversityService, ulong> UniversityServices { get; }
    IRepository<User, ulong> Users { get; }
    IRepository<Moderator, ulong> Moderators { get; }
    IRepository<UniversityServiceReport, ulong> Reports { get; }

    /// <summary>
    /// Сохраняет все изменения
    /// </summary>
    Task SaveChangesAsync();
}