using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Provides extension methods for mapping between client entities and models.
/// </summary>
public static class ClientMapper
{
    /// <summary>
    /// Maps a <see cref="Client"/> c to a model of type <typeparamref name="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to map to.</typeparam>
    /// <param name="client">The client c to map.</param>
    /// <returns>The mapped model.</returns>
    /// <exception cref="InvalidCastException">Thrown when the cast to <typeparamref name="TModel"/> fails.</exception>
    /// <exception cref="NotSupportedException">Thrown when the mapping to <typeparamref name="TModel"/> is not supported.</exception>
    public static TModel ToModel<TModel>(this Client client) where TModel : class
    {
        return client switch
        {
            Client c when typeof(TModel) == typeof(ClientModel) => ToClientModel(c) as TModel ?? throw new InvalidCastException(),
            _ => throw new NotSupportedException($"Mapping of type {typeof(TModel).Name} is not supported")
        };
    }

    /// <summary>
    /// Maps a <see cref="ClientModel"/> to a <see cref="Client"/> c.
    /// </summary>
    /// <param name="model">The client model to map.</param>
    /// <returns>The mapped client c.</returns>
    public static Client ToEntity(this ClientModel model)
    {
        return new Client
        {
            Id = model.ClientId,
            Name = model.Name,
            Address = model.Address,
            TenantId = model.TenantId.Value
        };
    }

    /// <summary>
    /// Maps a <see cref="CreateClientCommand"/> to a <see cref="Client"/> c.
    /// </summary>
    /// <param name="command">The create client command to map.</param>
    /// <returns>The mapped client c.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command name is null or empty.</exception>
    public static Client ToEntity(this CreateClientCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(command.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);

        return new Client
        {
            Id = ClientId.Create(Guid.NewGuid()),
            Name = command.Name,
            Address = command.Address
        };
    }

    /// <summary>
    /// Maps an <see cref="UpdateClientCommand"/> to a <see cref="Client"/> c.
    /// </summary>
    /// <param name="command">The update client command to map.</param>
    /// <returns>The mapped client c.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command name is null or empty.</exception>
    public static Client ToEntity(this UpdateClientCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(command.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);

        return new Client
        {
            Id = command.ClientId,
            Name = command.Name,
            Address = command.Address
        };
    }

    /// <summary>
    /// Maps a <see cref="Client"/> c to a <see cref="ResourceIdeaResponse{ClientModel}"/>.
    /// </summary>
    /// <param name="client">The client c to map.</param>
    /// <returns>The mapped resource idea response containing the client model.</returns>
    public static ResourceIdeaResponse<ClientModel> ToResourceIdeaResponse(this Client client)
    {
        return ResourceIdeaResponse<ClientModel>.Success(ToClientModel(client));
    }

    /// <summary>
    /// Maps a <see cref="PagedListResponse{Client}"/> to a <see cref="ResourceIdeaResponse{PagedListResponse{ClientModel}}"/>.
    /// </summary>
    /// <param name="clients">The paged list of client entities to map.</param>
    /// <returns>The mapped resource idea response containing the paged list of client models.</returns>
    public static ResourceIdeaResponse<PagedListResponse<ClientModel>> ToResourceIdeaResponse(this PagedListResponse<Client> clients)
    {
        return ResourceIdeaResponse<PagedListResponse<ClientModel>>.Success(ToModelPagedListResponse(clients));
    }

    /// <summary>
    /// Maps a <see cref="PagedListResponse{Client}"/> to a <see cref="PagedListResponse{ClientModel}"/>.
    /// </summary>
    /// <param name="clients">The paged list of client entities to map.</param>
    /// <returns>The mapped paged list of client models.</returns>
    private static PagedListResponse<ClientModel> ToModelPagedListResponse(PagedListResponse<Client> clients)
    {
        return new()
        {
            CurrentPage = clients.CurrentPage,
            PageSize = clients.PageSize,
            TotalCount = clients.TotalCount,
            Items = [.. clients.Items.Select(ToClientModel)]
        };
    }

    /// <summary>
    /// Maps a <see cref="Client"/> c to a <see cref="ClientModel"/>.
    /// </summary>
    /// <param name="client">The client c to map.</param>
    /// <returns>The mapped client model.</returns>
    private static ClientModel ToClientModel(Client client)
    {
        return new ClientModel
        {
            ClientId = client.Id,
            Name = client.Name,
            Address = client.Address
        };
    }
}
