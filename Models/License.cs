using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class License
    {
        public string LicenseKey { get; set; } = null!;
        public int LicenseTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CompanyCode { get; set; } = null!;

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual LicenseType LicenseType { get; set; } = null!;
    }
}
