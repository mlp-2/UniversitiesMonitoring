using System;
using System.Collections.Generic;

namespace UniversityMonitoring.Data.Models
{
    public partial class UniversityServiceStateChange
    {
        public ulong Id { get; set; }
        public ulong ServiceId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime ChangedAt { get; set; }

        public virtual UniversityService Service { get; set; } = null!;
    }
}