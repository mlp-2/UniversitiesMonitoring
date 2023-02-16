﻿namespace UniversityMonitoring.Data.Models
{
    public class UserRateOfService
    {
        public ulong Id { get; set; }
        public sbyte Rate { get; set; }
        public string? Comment { get; set; }
        public ulong AuthorId { get; set; }
        public ulong ServiceId { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual UniversityService Service { get; set; } = null!;
    }
}
