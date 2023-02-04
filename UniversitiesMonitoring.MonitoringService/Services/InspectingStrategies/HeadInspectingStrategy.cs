using System.Net;

namespace UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

internal class HeadInspectingStrategy : IInspectingStrategy
{
    public async Task<bool> ExecuteStrategyAsync(IPAddress ip)
    {
        try
        {
            var client = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Head, "http://" + ip);
            var response = await client.SendAsync(message);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}