// ======================================================================================
// File: GetEmployeeByIdQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetEmployeeByIdQuery.cs
// Description: Query to get a tenant employee by ID.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

public class GetEmployeeByIdQuery : BaseRequest<EmployeeModel>
{
    public EmployeeId EmployeeId { get; set; }

    /// <inheritdoc/>
    override public ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            EmployeeId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }.Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
