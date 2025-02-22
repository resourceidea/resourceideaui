// ----------------------------------------------------------------------------------
// File: CreateJobPositionCommand.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\JobPositions\Commands\CreateJobPositionCommand.cs
// Description: Command to create a job position.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Features.JobPositions.Commands;

/// <summary>
/// Command to create a job position.
/// </summary>
public sealed class CreateJobPositionCommand : BaseRequest<JobPositionModel>
{
    /// <summary>
    /// Title of the job position to create.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the job position to create.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Department identifier of the job position to create.
    /// </summary>
    public DepartmentId DepartmentId { get; set; }

    /// <summary>
    /// Maps the command to <see cref="JobPosition"> entity.
    /// </summary>
    /// <returns><see cref="JobPosition"/></returns>
    public JobPosition ToEntity()
    {
        return new JobPosition
        {
            Id = JobPositionId.Create(Guid.NewGuid()),
            Title = Title,
            Description = Description,
            DepartmentId = DepartmentId,
            TenantId = TenantId,
            Created = DateTimeOffset.UtcNow,
            LastModified = DateTimeOffset.UtcNow,
        };
    }

    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <returns><see cref="ValidationResponse"/></returns>
    public ValidationResponse Validate()
    {
        var validationFailureMessages = new[]
        {
            Title.ValidateRequired(nameof(Title)),
            Description.ValidateRequired(nameof(Description)),
            DepartmentId.ValidateRequired(),
            TenantId.ValidateRequired(),
        }
        .Where(message => !string.IsNullOrWhiteSpace(message));

        return validationFailureMessages.Any()
            ? new ValidationResponse(false, validationFailureMessages)
            : new ValidationResponse(true, []);
    }
}