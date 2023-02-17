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
        Url = universityServiceModel.Url;
    }
    
    [JsonPropertyName("serviceId")]
    public ulong ServiceId { get; }
    
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; }
    
    [JsonPropertyName("universityName")]
    public string UniversityName { get; }
    
    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; }
    public string Url { get; }
    public IEnumerable<UserEntity> Subscribers { get; }
    
    [JsonPropertyName("comments")]
    public IEnumerable<CommentEntity> Comments { get; }
}