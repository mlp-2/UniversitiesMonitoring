using System;
using System.Collections.Generic;

namespace UniversityMonitoring.Data.Models
{
    public partial class UniversityServiceReport
    {
        public ulong Id { get; set; }
        public string? Content { get; set; }
        public ulong ServiceId { get; set; }
        public ulong IssuerId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? AddedAt { get; set; }
        public bool IsSolved { get; set; }

        public virtual User Issuer { get; set; } = null!;
        public virtual UniversityService Service { get; set; } = null!;
    }
}
