// =============================================================================
// File: TenantEmployeesQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\TenantEmployeesQuery.cs
// Description: This file contains the definition of the TenantEmployeesQuery class.
// =============================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

/// <summary>
/// Query for tenant's employees.
/// </summary>
public sealed class TenantEmployeesQuery : BaseRequest<PagedListResponse<TenantEmployeeModel>>
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
