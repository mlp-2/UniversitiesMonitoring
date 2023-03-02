namespace UniversitiesMonitoring.Api;

internal struct UptimeData
{
    public UptimeData(double totalTime, double onlineTime)
    {
        TotalTime = totalTime;
        OnlineTime = onlineTime;
    }

    public double TotalTime { get; set; }
    public double OnlineTime { get; set; }
}