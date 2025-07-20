// =============================================================================
// File: GetEmployeesByJobPositionIdQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetEmployeesByJobPositionIdQuery.cs
// Description: This file contains the definition of the GetEmployeesByJobPositionIdQuery class.
// =============================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

/// <summary>
/// Query for getting employees by job position ID.
/// </summary>
public sealed class GetEmployeesByJobPositionIdQuery : BaseRequest<PagedListResponse<TenantEmployeeModel>>
{
    public JobPositionId JobPositionId { get; set; } = JobPositionId.Empty;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public override ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            TenantId.ValidateRequired(),
            JobPositionId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}