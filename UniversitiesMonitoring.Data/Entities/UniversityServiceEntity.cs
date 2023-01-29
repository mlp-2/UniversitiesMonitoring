using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class UniversityServiceEntity
{
    public UniversityServiceEntity(UniversityService universityServiceModel)
    {
        ServiceId = universityServiceModel.Id;
        ServiceName = universityServiceModel.Name;
        IsOnline = universityServiceModel.UniversityServiceStateChanges.FirstOrDefault()?.IsOnline ?? false;
        Subscribers = universityServiceModel.UserSubscribeToServices.Select(x => new UserEntity(x.User));
    }

    [JsonConstructor]
    public UniversityServiceEntity(ulong serviceId, string serviceName, bool isOnline, string ipAddress, UserEntity[] subscribers)
    {
        ServiceId = serviceId;
        ServiceName = serviceName;
        IsOnline = isOnline;
        IpAddress = ipAddress;
        Subscribers = subscribers;
    }
    
    public ulong ServiceId { get; }
    public string ServiceName { get; }
    public bool IsOnline { get; }
    public string IpAddress { get; }
    public IEnumerable<UserEntity> Subscribers { get; }
}