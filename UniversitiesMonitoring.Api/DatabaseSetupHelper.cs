using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api;

public class DatabaseSetupHelper
{
    private readonly UniversitiesMonitoringContext _dbContext;
    private readonly ILogger<DatabaseSetupHelper> _logger;

    public DatabaseSetupHelper(UniversitiesMonitoringContext dbContext, ILogger<DatabaseSetupHelper> logger)
    {
        _dbContext = dbContext;
        _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _logger = logger;
    }

    public async Task SetupDatabaseAsync()
    {
        await SetupDatabaseSchemaAsync();
        await SetupModeratorAsync();
    }

    private async Task SetupDatabaseSchemaAsync()
    {
        if (DatabaseExists())
        {
            _logger.LogDebug("DB already exists");
            return;
        }

        await _dbContext.Database.EnsureCreatedAsync();
        _logger.LogDebug("DB created");
    }

    private async Task SetupModeratorAsync()
    {
        if (await _dbContext.Moderators.CountAsync() > 0) return;

        var moderator = new Moderator()
        {
            Id = 1,
            PasswordSha256hash = Sha256Computing.ComputeSha256("123")
        };

        await _dbContext.Moderators.AddAsync(moderator);
        await _dbContext.SaveChangesAsync();
    }

    private bool DatabaseExists() => _dbContext.Database.GetService<IRelationalDatabaseCreator>().CanConnect() &&
                                     _dbContext.Database.GetService<IRelationalDatabaseCreator>().Exists() &&
                                     _dbContext.Database.GetService<IRelationalDatabaseCreator>().HasTables();
}