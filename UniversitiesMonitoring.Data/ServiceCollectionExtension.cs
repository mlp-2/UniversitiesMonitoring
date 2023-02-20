using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversityMonitoring.Data;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDataContext(this IServiceCollection collection, IConfiguration configuration) =>
        collection.AddDbContext<UniversitiesMonitoringContext>(options => 
            options.UseMySql(
                    Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? 
                        configuration.GetConnectionString("UniversitiesMonitoring"),
                    new MySqlServerVersion(new Version(8, 0, 29)))
                .UseLazyLoadingProxies())
            .AddScoped<IDataProvider, DataProvider>();
}