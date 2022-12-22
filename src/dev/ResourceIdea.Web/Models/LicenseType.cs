using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class LicenseType
    {
        public LicenseType()
        {
            Licenses = new HashSet<License>();
        }

        public int LicenseTypeId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Fee { get; set; }
        public string Plan { get; set; } = null!;

        public virtual ICollection<License> Licenses { get; set; }
    }
}
