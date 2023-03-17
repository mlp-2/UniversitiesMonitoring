namespace UniversityMonitoring.Data.Models
{
    public partial class UniversityService
    {
        public UniversityService()
        {
            ServiceResponseTimes = new HashSet<ServiceResponseTime>();
            UniversityServiceReports = new HashSet<UniversityServiceReport>();
            UniversityServiceStateChanges = new HashSet<UniversityServiceStateChange>();
            UserRateOfServices = new HashSet<UserRateOfService>();
            UserSubscribeToServices = new HashSet<UserSubscribeToService>();
        }

        public ulong Id { get; set; }
        public ulong UniversityId { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;

        public virtual University University { get; set; } = null!;
        public virtual ICollection<ServiceResponseTime> ServiceResponseTimes { get; set; }
        public virtual ICollection<UniversityServiceReport> UniversityServiceReports { get; set; }
        public virtual ICollection<UniversityServiceStateChange> UniversityServiceStateChanges { get; set; }
        public virtual ICollection<UserRateOfService> UserRateOfServices { get; set; }
        public virtual ICollection<UserSubscribeToService> UserSubscribeToServices { get; set; }
    }
}