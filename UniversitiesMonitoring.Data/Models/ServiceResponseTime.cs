using System;
using System.Collections.Generic;

namespace UniversityMonitoring.Data.Models
{
    public partial class ServiceResponseTime
    {
        public ulong Id { get; set; }
        public long ResponseTime { get; set; }
        public ulong ServiceId { get; set; }

        public virtual UniversityService Service { get; set; } = null!;
    }
}
