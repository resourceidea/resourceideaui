using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Project
    {
        public Project()
        {
            Jobs = new HashSet<Job>();
        }

        public string ProjectId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string? Color { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
