using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Responses;
using EastSeat.ResourceIdea.Domain.Tenants.Models;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Queries;

public sealed class GetTenantsListQuery : IRequest<ResourceIdeaResponse<PagedList<TenantModel>>>
{
    /// <summary>Current page number of the tenants paged list.</summary>
    public int CurrentPageNumber { get; set; } = 1;

    /// <summary>Size of the tenants paged list.</summary>
    public int PageSize { get; set; } = 10;

    /// <summary>Tenants query filter.</summary>
    public string Filter { get; set; } = string.Empty;
}
