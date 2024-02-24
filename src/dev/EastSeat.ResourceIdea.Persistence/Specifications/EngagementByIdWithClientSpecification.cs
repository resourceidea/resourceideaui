using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Domain.Entities;

using Optional;

namespace EastSeat.ResourceIdea.Persistence.Specifications;

/// <summary>
/// Specification for engagement by ID with the owning client.
/// </summary>
internal class EngagementByIdWithClientSpecification : Specification<Engagement>
{
    public EngagementByIdWithClientSpecification(Guid engagementId)
        : base(engagement => engagement.Id == engagementId)
    {
        AddInclude(engagement => engagement.Client);
    }
}
