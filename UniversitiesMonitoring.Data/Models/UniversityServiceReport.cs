namespace UniversityMonitoring.Data.Models
{
    public class UniversityServiceReport
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public long ServiceId { get; set; }
        public long IssuerId { get; set; }
        public bool IsOnline { get; set; }

        public User Issuer { get; set; } = null!;
        public UniversityService Service { get; set; } = null!;
    }
}
