namespace UniversityMonitoring.Data.Models
{
    public class UniversityService
    {
        public UniversityService()
        {
            UniversityServiceReports = new HashSet<UniversityServiceReport>();
            UniversityServiceStateChanges = new HashSet<UniversityServiceStateChange>();
            UserRateOfServices = new HashSet<UserRateOfService>();
            UserSubscribeToServices = new HashSet<UserSubscribeToService>();
        }

        public ulong Id { get; set; }
        public byte[] IpAddress { get; set; } = null!;
        public ulong UniversityId { get; set; }
        public string Name { get; set; } = null!;

        public virtual University University { get; set; } = null!;
        public virtual ICollection<UniversityServiceReport> UniversityServiceReports { get; set; }
        public virtual ICollection<UniversityServiceStateChange> UniversityServiceStateChanges { get; set; }
        public virtual ICollection<UserRateOfService> UserRateOfServices { get; set; }
        public virtual ICollection<UserSubscribeToService> UserSubscribeToServices { get; set; }
    }
}
