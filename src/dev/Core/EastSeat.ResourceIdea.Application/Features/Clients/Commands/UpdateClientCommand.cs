using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Commands;

/// <summary>
/// Command to update client.
/// </summary>
public sealed class UpdateClientCommand : IRequest<ResourceIdeaResponse<ClientModel>>
{
    /// <summary>DepartmentId of the client to update.</summary>
    public ClientId ClientId { get; set; }

    /// <summary>Client's name.</summary>
    public required string Name { get; set; }

    /// <summary>Client address.</summary>
    public Address Address { get; set; } = Address.Empty;

    /// <summary>Client owning tenant's DepartmentId.</summary>
    public TenantId TenantId { get; set; }    
}