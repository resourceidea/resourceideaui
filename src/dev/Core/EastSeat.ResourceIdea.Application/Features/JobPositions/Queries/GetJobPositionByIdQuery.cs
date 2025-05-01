// ------------------------------------------------------------------------------
// File: GetJobPositionByIdQuery.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/JobPositions/Queries/GetJobPositionByIdQuery.cs
// Description: Query for retrieving a single job position by ID
// ------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Queries;

/// <summary>
/// Query for retrieving job position by ID.
/// </summary>
/// <param name="JobPositionId">Job position identifier.</param>
public sealed class GetJobPositionByIdQuery() : BaseRequest<JobPositionModel>
{
    public JobPositionId JobPositionId { get; set; }

    /// <inheritdoc/>
    override public ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            JobPositionId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}