namespace UniversityMonitoring.Data.Models
{
    public class UserSubscribeToService
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ServiceId { get; set; }

        public UniversityService Service { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
