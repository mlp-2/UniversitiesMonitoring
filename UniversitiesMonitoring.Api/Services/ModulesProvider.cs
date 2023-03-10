using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UniversitiesMonitoring.Api.Entities;
using UniversityMonitoring.Data.Entities;
using UniversityMonitoring.Data.Models;
using UniversityMonitoring.Data.Repositories;

namespace UniversitiesMonitoring.Api.Services;

public class ModulesProvider : IModulesProvider
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
                var modules = await _dataProvider.MonitoringModules.GetlAll().AsNoTracking().ToArrayAsync();
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

    /// <inheritdoc />
    public async Task<ulong> CreateModuleAsync(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            throw new InvalidOperationException("Invalid URI");
        }

        var module = new MonitoringModule()
        {
            Url = url
        };
        
        await _dataProvider.MonitoringModules.AddAsync(module);
        await _dataProvider.SaveChangesAsync();
        return module.Id;
    }

    /// <inheritdoc />
    public async Task DeleteModuleAsync(ulong id)
    {
        var module = await _dataProvider.MonitoringModules.FindAsync(id);

        if (module == null)
        {
            throw new InvalidOperationException("Service hasn't found");
        }
        
        _dataProvider.MonitoringModules.Remove(module);
        await _dataProvider.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ModuleEntity> GetModulesAsync()
    {
        var allModules = _dataProvider.MonitoringModules.GetlAll().AsNoTracking();
        using var httpClient = new HttpClient();
        
        foreach (var module in allModules)
        {
            var location = await _cache.GetOrCreateAsync(GenerateLocationId(module), async entry =>
            {
                var moduleName = await GetModuleNameAsync(module.Url, httpClient);
                entry.Value = moduleName;

                return moduleName;
            });

            yield return new ModuleEntity(module.Id, location, module.Url);
        }
    }

    private static string GenerateCacheId(UniversityService service) => $"TEST_RESULT_{service.Id}";
    private static Uri GenerateTestUri(string url, UniversityService service) => new(url + $"/test?url={service.Url}");
    private static string GenerateLocationId(MonitoringModule module) => $"CACHED_LOCATION_{module.Id}";
    
    private static async Task<string?> GetModuleNameAsync(string url, HttpClient client)
    {
        try
        {
            var result = await client.GetAsync(url + "/location");
            if (!result.IsSuccessStatusCode) return null;
            var json = await result.Content.ReadFromJsonAsync<IDictionary<string, object>>();

            if (json == null) return null;
            
            return json["location"].ToString();
        }
        catch
        {
            return null;
        }   
    }
}