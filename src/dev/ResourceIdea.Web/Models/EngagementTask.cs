using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class EngagementTask
    {
        public EngagementTask()
        {
            JobResources = new HashSet<TaskAssignment>();
            JobSkills = new HashSet<JobSkill>();
            LineOfServiceJobs = new HashSet<LineOfServiceJob>();
        }

        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string EngagementId { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Manager { get; set; }
        public string? Partner { get; set; }

        public virtual Engagement Engagement { get; set; } = null!;
        public virtual ICollection<TaskAssignment> JobResources { get; set; }
        public virtual ICollection<JobSkill> JobSkills { get; set; }
        public virtual ICollection<LineOfServiceJob> LineOfServiceJobs { get; set; }
    }
}
