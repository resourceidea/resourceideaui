﻿using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Tenants.Commands;

/// <summary>
/// Command to update a tenant.
/// </summary>
public sealed class UpdateTenantCommand : IRequest<ResourceIdeaResponse<TenantModel>>
{
    /// <summary>Tenant's unique identifier. </summary>
    public TenantId TenantId { get; set; }

    /// <summary>Tenant's organization name. </summary>
    public string Organization { get; set; } = string.Empty;
}
