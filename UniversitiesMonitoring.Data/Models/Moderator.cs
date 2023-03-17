namespace UniversityMonitoring.Data.Models
{
    public partial class Moderator
    {
        public ulong Id { get; set; }
        public byte[] PasswordSha256hash { get; set; } = null!;
    }
}