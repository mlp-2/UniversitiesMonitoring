namespace UniversityMonitoring.Data.Models
{
    public class UserSubscribeToService
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public ulong ServiceId { get; set; }

        public UniversityService Service { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
