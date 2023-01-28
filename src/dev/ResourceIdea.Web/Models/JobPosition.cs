using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class JobPosition
    {
        public JobPosition()
        {
            Resources = new HashSet<Resource>();
        }

        public int JobPositionId { get; set; }
        public string PositionTitle { get; set; } = null!;
        public int JobLevel { get; set; }
        public string CompanyCode { get; set; } = null!;

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual ICollection<Resource> Resources { get; set; }
    }
}
