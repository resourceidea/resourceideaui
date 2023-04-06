using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Engagement
    {
        public Engagement()
        {
            Jobs = new HashSet<EngagementTask>();
        }

        public string EngagementId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string? Color { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<EngagementTask> Jobs { get; set; }
    }
}
