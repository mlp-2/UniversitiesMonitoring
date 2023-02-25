using UniversitiesMonitoring.Module.Networking.Testing;
using UniversityMonitoring.Data.Entities;

namespace UniversitiesMonitoring.Module.Networking;

public class TestProvider
{
    private readonly IEnumerable<ITestStrategy> _testStrategies;
    private readonly string _locationName;

    public TestProvider(IEnumerable<ITestStrategy> testStrategies,
                        IConfiguration configuration)
    {
        _testStrategies = testStrategies;
        _locationName = Environment.GetEnvironmentVariable("LOCATION_NAME") ?? configuration["LocationName"] ?? "Неизвестная локация";
    }

    /// <summary>
    /// Тестирует ресурс
    /// </summary>
    /// <param name="testUrl">Url ресурса</param>
    /// <returns>Отчет о ресурсе</returns>
    public async Task<TestReport> TestAsync(Uri testUrl)
    {
        var reportBuilder = new TestReportBuilder();
        
        reportBuilder.WithTestLocation(_locationName);
        
        foreach (var testStrategy in _testStrategies)
        {
            await testStrategy.TestAsync(testUrl, reportBuilder);
        }

        return reportBuilder.Build();
    }
}