// ======================================================================================
// File: GetEmployeeByApplicationUserIdQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetEmployeeByApplicationUserIdQuery.cs
// Description: Query to get an employee by ApplicationUserId.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

public class GetEmployeeByApplicationUserIdQuery : BaseRequest<EmployeeModel>
{
    public ApplicationUserId ApplicationUserId { get; set; }

    /// <inheritdoc/>
    override public ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            ApplicationUserId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }.Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}