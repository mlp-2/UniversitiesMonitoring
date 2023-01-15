namespace UniversityMonitoring.Data.Models
{
    public class Moderator
    {
        public ulong Id { get; set; }
        public byte[] PasswordSha256hash { get; set; } = null!;
    }
}
