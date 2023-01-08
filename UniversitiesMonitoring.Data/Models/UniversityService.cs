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

        public long Id { get; set; }
        public byte[] Ipaddress { get; set; } = null!;
        public long UniversityId { get; set; }

        public University University { get; set; } = null!;
        public ICollection<UniversityServiceReport> UniversityServiceReports { get; set; }
        public ICollection<UniversityServiceStateChange> UniversityServiceStateChanges { get; set; }
        public ICollection<UserRateOfService> UserRateOfServices { get; set; }
        public ICollection<UserSubscribeToService> UserSubscribeToServices { get; set; }
    }
}
