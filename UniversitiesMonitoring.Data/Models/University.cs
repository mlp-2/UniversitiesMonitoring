namespace UniversityMonitoring.Data.Models
{
    public class University
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
