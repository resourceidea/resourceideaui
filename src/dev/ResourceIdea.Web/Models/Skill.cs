using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Skill
    {
        public Skill()
        {
            JobSkills = new HashSet<JobSkill>();
            ResourceSkills = new HashSet<ResourceSkill>();
        }

        public string SkillId { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<JobSkill> JobSkills { get; set; }
        public virtual ICollection<ResourceSkill> ResourceSkills { get; set; }
    }
}
