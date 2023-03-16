using System.Text.Json.Serialization;

namespace UniversityMonitoring.Data.Entities;

public class ServiceStatisticsEntity
{
   [JsonConstructor]
   public ServiceStatisticsEntity(ulong serviceId, long responseTime)
   {
      ServiceId = serviceId;
      ResponseTime = responseTime;
   }

   public ulong ServiceId { get; }
   public long? ResponseTime { get; }
}