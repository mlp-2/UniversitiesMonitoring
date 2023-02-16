using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class UniversityServiceEntity
{
    public UniversityServiceEntity(UniversityService universityServiceModel, bool loadUsers = true, bool loadComments = true, bool? isSubscribed = null)
    { 
        ServiceId = universityServiceModel.Id;
        ServiceName = universityServiceModel.Name;
        UniversityName = universityServiceModel.University.Name;
        IsOnline = universityServiceModel.UniversityServiceStateChanges.LastOrDefault()?.IsOnline ?? false;
        Subscribers = loadUsers ? universityServiceModel.UserSubscribeToServices.Select(x => new UserEntity(x.User)) : Array.Empty<UserEntity>();
        Comments = loadComments ? universityServiceModel.UserRateOfServices.Select(x => new CommentEntity(x)) : Array.Empty<CommentEntity>();
        IpAddress = $"{universityServiceModel.IpAddress[0]}:" +
                    $"{universityServiceModel.IpAddress[1]}:" +
                    $"{universityServiceModel.IpAddress[2]}:" +
                    $"{universityServiceModel.IpAddress[3]}:" +
                    $"{universityServiceModel.IpAddress[4]}:" +
                    $"{universityServiceModel.IpAddress[5]}";
        IsSubscribed = isSubscribed;
    }

    [JsonConstructor]
    public UniversityServiceEntity(ulong serviceId,
        string serviceName,
        string universityName,
        bool isOnline,
        string ipAddress,
        IEnumerable<UserEntity> subscribers,
        IEnumerable<CommentEntity> comments)
    {
        ServiceId = serviceId;
        ServiceName = serviceName;
        UniversityName = universityName;
        IsOnline = isOnline;
        IpAddress = ipAddress;
        Subscribers = subscribers;
        Comments = comments;
    }
    
    [JsonPropertyName("serviceId")]
    public ulong ServiceId { get; }
    
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; }
    
    [JsonPropertyName("universityName")]
    public string UniversityName { get; }
    
    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; }
    
    [JsonPropertyName("ipAddress")]
    public string IpAddress { get; }
    
    [JsonPropertyName("subscribers")]
    public IEnumerable<UserEntity> Subscribers { get; }
    
    [JsonPropertyName("comments")]
    public IEnumerable<CommentEntity> Comments { get; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("isSubscribed")]
    public bool? IsSubscribed { get; }
}