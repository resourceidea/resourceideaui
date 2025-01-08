using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Queries;

/// <summary>
/// Query to get a client by DepartmentId.
/// </summary>
public sealed class GetClientByIdQuery : IRequest<ResourceIdeaResponse<ClientModel>>
{
    /// <summary>Client DepartmentId.</summary>
    public ClientId ClientId { get; set; }
}