using System;
using System.Collections.Generic;

namespace ResourceIdea.Models
{
    public partial class RoleGroupingMember
    {
        public Guid RoleGroupingMemberId { get; set; }
        public Guid RoleGroupingId { get; set; }
        public string RoleId { get; set; } = null!;
        public byte[] RowVersion { get; set; } = null!;

        public virtual RoleGrouping RoleGrouping { get; set; } = null!;
    }
}
