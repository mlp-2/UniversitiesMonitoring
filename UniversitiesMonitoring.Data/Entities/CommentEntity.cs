using System.Text.Json.Serialization;
using UniversityMonitoring.Data.Models;

namespace UniversityMonitoring.Data.Entities;

public class CommentEntity
{
    public CommentEntity(UserRateOfService rate)
    {
        Author = new UserEntity(rate.Author);
        Content = rate.Comment;
        Rate = rate.Rate;
    }

    [JsonPropertyName("author")] public UserEntity Author { get; }
    [JsonPropertyName("content")] public string? Content { get; }
    [JsonPropertyName("rate")] public sbyte Rate { get; }
}