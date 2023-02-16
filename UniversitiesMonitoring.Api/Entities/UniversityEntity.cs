using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversitiesMonitoring.Api.Entities;

public class UniversityEntity
{
    public UniversityEntity(University university)
    {
        Id = university.Id;
        Name = university.Name;
        IsOnline = true;

        foreach (var service in university.UniversityServices)
        {
            IsOnline &= service.UniversityServiceStateChanges.LastOrDefault()?.IsOnline ?? false; 
            
            foreach (var rate in service.UserRateOfServices)
            {
                Rating += rate.Rate;
                CommentsCount += 1;    
            }
        }

        if (CommentsCount > 0) Rating /= CommentsCount;
        else Rating = 0;
    }

    [JsonPropertyName("id")]
    public ulong Id { get; }
    
    [JsonPropertyName("name")]
    public string Name { get; }
    
    [JsonPropertyName("commentsCount")]
    public int CommentsCount { get; }
    
    [JsonPropertyName("rating")]
    public double Rating { get; }
    
    [JsonPropertyName("isOnline")]
    public bool IsOnline { get; }
    
    [JsonPropertyName("isSubscribed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsSubscribed { get; set; }
}