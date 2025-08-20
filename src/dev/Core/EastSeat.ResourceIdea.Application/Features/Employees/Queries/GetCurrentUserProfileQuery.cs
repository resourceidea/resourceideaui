// ======================================================================================
// File: GetCurrentUserProfileQuery.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Queries\GetCurrentUserProfileQuery.cs
// Description: Query to get current user's profile information.
// ======================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Queries;

public class GetCurrentUserProfileQuery : BaseRequest<TenantEmployeeModel>
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