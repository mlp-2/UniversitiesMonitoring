namespace UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

internal class HeadInspectingStrategy : IInspectingStrategy
{
    public async Task<bool> ExecuteStrategyAsync(Uri url)
    {
        try
        {
            var client = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Head, url);
            var response = await client.SendAsync(message);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}