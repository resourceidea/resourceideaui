using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Job
    {
        public Job()
        {
            JobResources = new HashSet<JobResource>();
            JobSkills = new HashSet<JobSkill>();
            LineOfServiceJobs = new HashSet<LineOfServiceJob>();
        }

        public string JobId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ProjectId { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Manager { get; set; }
        public string? Partner { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ICollection<JobResource> JobResources { get; set; }
        public virtual ICollection<JobSkill> JobSkills { get; set; }
        public virtual ICollection<LineOfServiceJob> LineOfServiceJobs { get; set; }
    }
}
