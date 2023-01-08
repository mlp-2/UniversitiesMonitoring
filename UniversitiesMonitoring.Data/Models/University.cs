namespace UniversityMonitoring.Data.Models
{
    public partial class University
    {
        public University()
        {
            UniversityServices = new HashSet<UniversityService>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<UniversityService> UniversityServices { get; set; }
    }
}
