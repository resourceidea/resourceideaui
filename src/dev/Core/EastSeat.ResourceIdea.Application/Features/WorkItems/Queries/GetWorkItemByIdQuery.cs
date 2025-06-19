using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Queries;

/// <summary>
/// Query to retrieve a work item by its identifier.
/// </summary>
public sealed class GetWorkItemByIdQuery : BaseRequest<WorkItemModel>
{
    /// <summary>
    /// The identifier of the work item to retrieve.
    /// </summary>
    public WorkItemId WorkItemId { get; set; }

    /// <summary>
    /// Validates the request.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/> instance.</returns>
    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            WorkItemId.Value != Guid.Empty ? string.Empty : "Work item identifier cannot be empty.",
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}