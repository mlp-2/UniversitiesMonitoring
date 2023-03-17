namespace UniversityMonitoring.Data.Models
{
    public partial class University
    {
        public University()
        {
            UniversityServices = new HashSet<UniversityService>();
        }

        public ulong Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<UniversityService> UniversityServices { get; set; }
    }
}