using Microsoft.EntityFrameworkCore;

namespace UniversityMonitoring.Data.Models;

public partial class UniversitiesMonitoringContext
{
    public void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UniversityServiceReport>().Navigation(x => x.Service);
        modelBuilder.Entity<UniversityServiceReport>().Navigation(x => x.Issuer);

        modelBuilder.Entity<UniversityService>().Navigation(x => x.University);
        modelBuilder.Entity<UniversityService>().Navigation(x => x.UniversityServiceReports);
        modelBuilder.Entity<UniversityService>().Navigation(x => x.UniversityServiceStateChanges);
        modelBuilder.Entity<UniversityService>().Navigation(x => x.UserRateOfServices);
        modelBuilder.Entity<UniversityService>().Navigation(x => x.UserSubscribeToServices);

        modelBuilder.Entity<University>().Navigation(x => x.UniversityServices);

        modelBuilder.Entity<UniversityServiceStateChange>().Navigation(x => x.Service);

        modelBuilder.Entity<User>().Navigation(x => x.UserSubscribeToServices);
        modelBuilder.Entity<User>().Navigation(x => x.UserRateOfServices);
        modelBuilder.Entity<User>().Navigation(x => x.UniversityServiceReports);

        modelBuilder.Entity<UserRateOfService>().Navigation(x => x.Service);
        modelBuilder.Entity<UserRateOfService>().Navigation(x => x.Author);
        
        modelBuilder.Entity<UserSubscribeToService>().Navigation(x => x.Service);
        modelBuilder.Entity<UserSubscribeToService>().Navigation(x => x.User);
    }
}