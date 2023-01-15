namespace UniversityMonitoring.Data.Models
{
    public class UniversityServiceStateChange
    {
        public ulong Id { get; set; }
        public ulong ServiceId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime ChangedAt { get; set; }

        public UniversityService Service { get; set; } = null!;
    }
}
