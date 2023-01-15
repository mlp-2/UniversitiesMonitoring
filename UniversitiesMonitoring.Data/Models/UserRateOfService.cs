namespace UniversityMonitoring.Data.Models
{
    public class UserRateOfService
    {
        public ulong Id { get; set; }
        public sbyte Rate { get; set; }
        public string? Comment { get; set; }
        public ulong AuthorId { get; set; }
        public ulong ServiceId { get; set; }

        public User Author { get; set; } = null!;
        public UniversityService Service { get; set; } = null!;
    }
}
