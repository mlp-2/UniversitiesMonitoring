using System.Text.Json.Serialization;

namespace UniversitiesMonitoring.Api.Entities;

internal class TelegramUpdateEntity
{
    [JsonConstructor]
    public TelegramUpdateEntity(string telegramTag, bool canSend)
    {
        TelegramTag = telegramTag;
        CanSend = canSend;
    }

    public string TelegramTag { get; }
    public bool CanSend { get; }
}