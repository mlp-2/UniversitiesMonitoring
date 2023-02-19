using System;
using System.Collections.Generic;

namespace UniversityMonitoring.Data.Models
{
    public partial class UserSubscribeToService
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public ulong ServiceId { get; set; }

        public virtual UniversityService Service { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
