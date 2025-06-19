using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;

public sealed class GetWorkItemsByEngagementQuery(EngagementId engagementId, int pageNumber, int pageSize, string filter = "")
    : BaseRequest<PagedListResponse<WorkItemModel>>
{
    public EngagementId EngagementId { get; } = engagementId;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public string Filter { get; } = filter;

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