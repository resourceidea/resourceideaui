using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class RoleGrouping
    {
        public RoleGrouping()
        {
            RoleGroupingMembers = new HashSet<RoleGroupingMember>();
        }

        public Guid RoleGroupingId { get; set; }
        public string Description { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;
        public string CompanyCode { get; set; } = null!;

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual ICollection<RoleGroupingMember> RoleGroupingMembers { get; set; }
    }
}
