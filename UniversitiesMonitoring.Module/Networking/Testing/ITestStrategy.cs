namespace UniversitiesMonitoring.Module.Networking.Testing;

/// <summary>
/// Интерфейс, который тестирует определённый ресурс
/// </summary>
public interface ITestStrategy
{
    /// <summary>
    /// Проводит тест ресурса
    /// </summary>
    /// <param name="url">Url ресурса</param>
    /// <param name="reportBuilder">Билдер для построения отчета</param>
    Task TestAsync(Uri url, TestReportBuilder reportBuilder);
}