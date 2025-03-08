// ----------------------------------------------------------------------------------
// File: GetAllJobPositionsQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Queries\GetAllJobPositionsQuery.cs
// Description: Query to get all job positions with department information.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;

/// <summary>
/// Query to get all job positions with department information.
/// </summary>
public class GetAllJobPositionsQuery : BaseRequest<PagedListResponse<TenantJobPositionModel>>
{
    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; } = 50;

    /// <summary>
    /// Validates the query.
    /// </summary>
    /// <returns>Validation response.</returns>
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