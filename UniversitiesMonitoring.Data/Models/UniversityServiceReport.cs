namespace UniversityMonitoring.Data.Models
{
    public class UniversityServiceReport
    {
        public ulong Id { get; set; }
        public string? Content { get; set; }
        public ulong ServiceId { get; set; }
        public ulong IssuerId { get; set; }
        public bool IsOnline { get; set; }

        public User Issuer { get; set; } = null!;
        public UniversityService Service { get; set; } = null!;
    }
}
