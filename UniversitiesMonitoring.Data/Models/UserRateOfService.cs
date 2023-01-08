namespace UniversityMonitoring.Data.Models
{
    public class UserRateOfService
    {
        public long Id { get; set; }
        public sbyte Rate { get; set; }
        public string? Comment { get; set; }
        public long AuthorId { get; set; }
        public long ServiceId { get; set; }

        public User Author { get; set; } = null!;
        public UniversityService Service { get; set; } = null!;
    }
}
