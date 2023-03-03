using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Services;

internal class ModulesProvider : IModulesProvider
{
    private readonly IDataProvider _dataProvider;
    private readonly IMemoryCache _cache;

    public ModulesProvider(IDataProvider dataProvider, IMemoryCache cache)
    {
        _dataProvider = dataProvider;
        _cache = cache;
    }
    
    /// <inheritdoc />
    public Task<IEnumerable<TestReport>> TestServiceAsync(UniversityService service) =>
        _cache.GetOrCreateAsync<IEnumerable<TestReport>>(GenerateCacheId(service),
            async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);

                using var httpClient = new HttpClient();
                var modules = await _dataProvider.MonitoringModules.GetlAll().ToArrayAsync();
                var reports = new List<TestReport>();
                
                foreach (var module in modules)
                {
                    try
                    {
                        var result = await httpClient.GetFromJsonAsync<TestReport>(GenerateTestUri(module.Url, service));
                        
                        if (result != null) reports.Add(result);
                    }
                    catch 
                    {
                        // ignored
                    }
                }

                return reports;
            });

    private static string GenerateCacheId(UniversityService service) => $"TEST_RESULT_{service.Id}";
    private static Uri GenerateTestUri(string url, UniversityService service) => new(url + $"/test?url={service.Url}");
}