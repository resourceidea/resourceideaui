using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Client
    {
        public Client()
        {
            Projects = new HashSet<Project>();
        }

        public string ClientId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Industry { get; set; }
        public string CompanyCode { get; set; } = null!;
        public bool? Active { get; set; }

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual ICollection<Project> Projects { get; set; }
    }
}
