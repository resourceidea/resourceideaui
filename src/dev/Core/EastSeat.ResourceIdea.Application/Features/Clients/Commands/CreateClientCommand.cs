using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Commands;

/// <summary>
/// Command to create a client.
/// </summary>
public class CreateClientCommand : IRequest<ResourceIdeaResponse<ClientModel>>
{
    /// <summary>Client name</summary>
    public required string Name { get; set; }

    /// <summary>Client address</summary>
    public Address Address { get; set; } = Address.Empty;

    /// <summary>Client's owning tenant</summary>
    public TenantId TenantId { get; set; }
}