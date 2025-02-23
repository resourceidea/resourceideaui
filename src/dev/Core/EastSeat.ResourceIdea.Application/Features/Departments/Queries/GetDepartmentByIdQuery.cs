using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Departments.Models;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.Departments.Queries;

/// <summary>
/// Query to get a department given its ID.
/// </summary>
public sealed class GetDepartmentByIdQuery : BaseRequest<DepartmentModel>
{
    /// <summary>Department ID.</summary>
    public DepartmentId DepartmentId { get; set; }

    /// <inheritdoc/>
    override public ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            DepartmentId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}
