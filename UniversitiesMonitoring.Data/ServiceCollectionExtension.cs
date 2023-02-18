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
                    configuration.GetConnectionString("UniversitiesMonitoring"),
                    new MySqlServerVersion(new Version(8, 0, 29)))
                .UseLazyLoadingProxies()
                .LogTo(Console.WriteLine))
            .AddScoped<IDataProvider, DataProvider>();
}