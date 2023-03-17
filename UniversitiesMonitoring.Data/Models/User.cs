namespace UniversityMonitoring.Data.Models
{
    public partial class User
    {
        public User()
        {
            UniversityServiceReports = new HashSet<UniversityServiceReport>();
            UserRateOfServices = new HashSet<UserRateOfService>();
            UserSubscribeToServices = new HashSet<UserSubscribeToService>();
        }

        public ulong Id { get; set; }
        public string Username { get; set; } = null!;
        public byte[] PasswordSha256hash { get; set; } = null!;
        public string? Email { get; set; }
        public bool SendEmailNotification { get; set; }

        public virtual ICollection<UniversityServiceReport> UniversityServiceReports { get; set; }
        public virtual ICollection<UserRateOfService> UserRateOfServices { get; set; }
        public virtual ICollection<UserSubscribeToService> UserSubscribeToServices { get; set; }
    }
}