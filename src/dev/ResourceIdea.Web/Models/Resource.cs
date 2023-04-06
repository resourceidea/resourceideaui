using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Resource
    {
        public Resource()
        {
            JobResources = new HashSet<TaskAssignment>();
            ResourceSkills = new HashSet<ResourceSkill>();
        }

        public string ResourceId { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CompanyCode { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? JobPositionId { get; set; }
        public string? JobsManagedColor { get; set; }

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual JobPosition? JobPosition { get; set; }
        public virtual ICollection<TaskAssignment> JobResources { get; set; }
        public virtual ICollection<ResourceSkill> ResourceSkills { get; set; }
    }
}
