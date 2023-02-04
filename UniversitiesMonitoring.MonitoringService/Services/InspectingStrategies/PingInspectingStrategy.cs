using System.Net.NetworkInformation;
using System.Net;

namespace UniversitiesMonitoring.MonitoringService.Services.InspectingStrategies;

internal class PingInspectingStrategy : IInspectingStrategy
{
    public async Task<bool> ExecuteStrategyAsync(IPAddress ip)
    {
        using var ping = new Ping();
        var reply = await ping.SendPingAsync(ip);
        return reply.Status == IPStatus.Success;
    }
}