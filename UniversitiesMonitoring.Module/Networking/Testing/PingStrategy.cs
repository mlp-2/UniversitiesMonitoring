using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace UniversitiesMonitoring.Module.Networking.Testing;

internal class PingStrategy : ITestStrategy
{
    private static readonly byte[] Data32Bytes = Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

    private static readonly PingOptions PingOptions = new()
    {
        DontFragment = true
    };
    
    /// <inheritdoc />
    public async Task TestAsync(Uri url, TestReportBuilder reportBuilder)
    {
        try
        {
            using var ping = new Ping();

            var ipEntry = await Dns.GetHostEntryAsync(url.Host);
            var reply = await ping.SendPingAsync(ipEntry.AddressList[0], 1000, Data32Bytes, PingOptions);

            if (reply.Status != IPStatus.Success) return;
        
            reportBuilder.WithPingResult(reply.RoundtripTime);
        }
        catch 
        {
            // ignored
        }
    }
}