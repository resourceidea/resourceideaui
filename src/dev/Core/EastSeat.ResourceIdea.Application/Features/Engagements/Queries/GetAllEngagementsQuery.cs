using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Queries;

public sealed class GetAllEngagementsQuery (int pageNumber, int pageSize)
    : BaseRequest<PagedListResponse<EngagementModel>>
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;

    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            PageNumber > 0 ? string.Empty : "Page number must be greater than zero.",
            PageSize > 0 ? string.Empty : "Page size must be greater than zero.",
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
