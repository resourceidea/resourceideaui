// =========================================================================================
// File: Client.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\Clients\Entities\Client.cs
// Description: Client entity representing a client in the system.
// =========================================================================================

using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.Clients.Entities;

public class Client : BaseEntity
{
    /// <summary>
    /// Client ID.
    /// </summary>
    public ClientId Id { get; set; }

    /// <summary>
    /// Client address.
    /// </summary>
    public Address Address { get; set; } = Address.Empty;

    /// <summary>
    /// Client name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Checks if the instance of <see cref="Client"/> is empty.
    /// </summary>
    /// <returns>True if instance is empty; Otherwise, False.</returns>
    public bool IsEmpty() => this == EmptyClient.Instance;

    public override TModel ToModel<TModel>() where TModel : class =>
        typeof(TModel) switch
        {
            var t when t == typeof(ClientModel) => (TModel)(object)MapToClientModel(),
            var t when t == typeof(TenantClientModel) => (TModel)(object)MapToTenantClientModel(),
            _ => throw new InvalidOperationException($"Mapping for {typeof(TModel).Name} is not configured."),
        };

    private TenantClientModel MapToTenantClientModel() => new(Id, Address, Name);

    private ClientModel MapToClientModel() => new(Id, TenantId, Name, Address);

    public override ResourceIdeaResponse<TModel> ToResourceIdeaResponse<TEntity, TModel>() =>
        typeof(TModel) switch
        {
            var t when t == typeof(ClientModel) => ResourceIdeaResponse<TModel>.Success(ToModel<TModel>()),
            var t when t == typeof(TenantClientModel) => ResourceIdeaResponse<TModel>.Success(ToModel<TModel>()),
            _ => throw new InvalidOperationException($"Cannot map {typeof(TEntity).Name} to {typeof(TModel).Name}")
        };
}