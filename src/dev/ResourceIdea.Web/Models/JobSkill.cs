using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class JobSkill
    {
        public int Id { get; set; }
        public string JobId { get; set; } = null!;
        public string SkillId { get; set; } = null!;

        public virtual EngagementTask Job { get; set; } = null!;
        public virtual Skill Skill { get; set; } = null!;
    }
}
