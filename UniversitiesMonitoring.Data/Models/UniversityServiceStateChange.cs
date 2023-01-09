namespace UniversityMonitoring.Data.Models
{
    public class UniversityServiceStateChange
    {
        public long Id { get; set; }
        public long ServiceId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime ChangedAt { get; set; }

        public UniversityService Service { get; set; } = null!;
    }
}
