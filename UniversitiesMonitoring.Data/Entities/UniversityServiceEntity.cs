using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class UniversityServiceEntity
{
    public UniversityServiceEntity(UniversityService universityServiceModel, bool loadUsers = true, bool loadComments = true)
    { 
        ServiceId = universityServiceModel.Id;
        ServiceName = universityServiceModel.Name;
        IsOnline = universityServiceModel.UniversityServiceStateChanges.FirstOrDefault()?.IsOnline ?? false;
        Subscribers = loadUsers ? universityServiceModel.UserSubscribeToServices.Select(x => new UserEntity(x.User)) : Array.Empty<UserEntity>();
        Comments = loadComments ? universityServiceModel.UserRateOfServices.Select(x => new CommentEntity(x)) : Array.Empty<CommentEntity>();
        Url = universityServiceModel.Url;
    }

    public ulong ServiceId { get; }
    public string ServiceName { get; }
    public bool IsOnline { get; }
    public string Url { get; }
    public IEnumerable<UserEntity> Subscribers { get; }
    public IEnumerable<CommentEntity> Comments { get; }
}