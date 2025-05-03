using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Clients.Queries;

/// <summary>
/// Query to get a list of clients.
/// </summary>
public sealed class TenantClientsQuery : BaseRequest<ResourceIdeaResponse<PagedListResponse<ClientModel>>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            TenantId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
