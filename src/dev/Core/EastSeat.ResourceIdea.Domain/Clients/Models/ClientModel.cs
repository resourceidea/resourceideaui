using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.Clients.Models;

/// <summary>
/// Client model.
/// </summary>
public record ClientModel(ClientId ClientId, TenantId TenantId, string Name, Address Address);
