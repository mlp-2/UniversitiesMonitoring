using System.Diagnostics;

namespace UniversitiesMonitoring.Module.Networking.Testing;

internal class HeadStrategy : ITestStrategy
{
    /// <inheritdoc />
    public async Task TestAsync(Uri url, TestReportBuilder reportBuilder)
    {
        try
        {
            using var httpClient = new HttpClient();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = await httpClient.SendAsync(BuildRequestMessage(url));
            stopwatch.Stop();

            if (!result.IsSuccessStatusCode) return;

            reportBuilder.WithHeadResult(stopwatch.ElapsedMilliseconds);
        }
        catch
        {
            // ignored
        }
    }

    private static HttpRequestMessage BuildRequestMessage(Uri resourceAddress) => new(HttpMethod.Head, resourceAddress);
}