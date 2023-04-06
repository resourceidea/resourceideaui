using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class TaskAssignment
    {
        public int Id { get; set; }
        public string TaskId { get; set; } = null!;
        public string ResourceId { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Details { get; set; }

        public virtual EngagementTask Task { get; set; } = null!;
        public virtual Resource Resource { get; set; } = null!;
    }
}
