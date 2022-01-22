using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class LineOfService
    {
        public LineOfService()
        {
            LineOfServiceJobs = new HashSet<LineOfServiceJob>();
        }

        public Guid LineOfServiceId { get; set; }
        public string Description { get; set; } = null!;
        public string CompanyCode { get; set; } = null!;

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual ICollection<LineOfServiceJob> LineOfServiceJobs { get; set; }
    }
}
