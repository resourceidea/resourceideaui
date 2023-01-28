using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class ResourceSkill
    {
        public int Id { get; set; }
        public string ResourceId { get; set; } = null!;
        public string SkillId { get; set; } = null!;
        public double Score { get; set; }

        public virtual Resource Resource { get; set; } = null!;
        public virtual Skill Skill { get; set; } = null!;
    }
}
