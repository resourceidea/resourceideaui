using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Queries;

/// <summary>
/// Query to get a client by DepartmentId.
/// </summary>
public sealed class GetClientByIdQuery : BaseRequest<ClientModel>
{
    /// <summary>Client DepartmentId.</summary>
    public ClientId ClientId { get; set; }

    /// <inheritdoc/>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            ClientId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}