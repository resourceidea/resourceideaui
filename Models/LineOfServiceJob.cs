using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class LineOfServiceJob
    {
        public Guid Id { get; set; }
        public Guid LineOfServiceId { get; set; }
        public string JobId { get; set; } = null!;

        public virtual Job Job { get; set; } = null!;
        public virtual LineOfService LineOfService { get; set; } = null!;
    }
}
