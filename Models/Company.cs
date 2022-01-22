using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class Company
    {
        public Company()
        {
            Clients = new HashSet<Client>();
            JobPositions = new HashSet<JobPosition>();
            Licenses = new HashSet<License>();
            LineOfServices = new HashSet<LineOfService>();
            Resources = new HashSet<Resource>();
            RoleGroupings = new HashSet<RoleGrouping>();
        }

        public string CompanyCode { get; set; } = null!;
        public string OrganizationName { get; set; } = null!;

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<JobPosition> JobPositions { get; set; }
        public virtual ICollection<License> Licenses { get; set; }
        public virtual ICollection<LineOfService> LineOfServices { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<RoleGrouping> RoleGroupings { get; set; }
    }
}
