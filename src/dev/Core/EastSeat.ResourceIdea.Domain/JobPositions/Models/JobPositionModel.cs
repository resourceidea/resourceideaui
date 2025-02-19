// ----------------------------------------------------------------------------------
// File: JobPositionModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\JobPositions\Models\JobPositionModel.cs
// Description: Defines the model for a job position.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Extensions;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Domain.JobPositions.Models
{
    /// <summary>
    /// Job position model.
    /// </summary>
    public class JobPositionModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobPositionModel"/> class.
        /// </summary>
        public JobPositionModel()
        {
        }

        /// <summary>
        /// Gets or sets the job position identifier.
        /// </summary>
        public JobPositionId Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the job position.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the job position.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the department identifier.
        /// </summary>
        public DepartmentId DepartmentId { get; set; }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <returns><see cref="ValidationResponse"/></returns>
        public ValidationResponse IsValid()
        {
            var validationFailureMessages = new[]
            {
                Title.ValidateRequired(nameof(Title)),
                Description.ValidateRequired(nameof(Description)),
                DepartmentId.ValidateRequired(),
            }
            .Where(message => !string.IsNullOrWhiteSpace(message));

            return validationFailureMessages.Any()
                ? new ValidationResponse(false, validationFailureMessages)
                : new ValidationResponse(true, []);
        }

        /// <summary>
        /// Maps the <see cref="JobPositionModel"/> to <see cref="JobPosition"/>.
        /// </summary>
        /// <returns><see cref="JobPosition"/> entity.</returns>
        public JobPosition ToEntity()
        {
            return new JobPosition
            {
                Title = Title,
                Description = Description,
                DepartmentId = DepartmentId,
                Id = Id
            };
        }

        /// <summary>
        /// Maps the <see cref="JobPositionModel"/> to <see cref="ResourceIdeaResponse{JobPositionModel}"/>.
        /// </summary>
        /// <returns><see cref="ResourceIdeaResponse{JobPositionModel}"/></returns>
        public ResourceIdeaResponse<JobPositionModel> ToResourceIdeaResponse()
        {
            throw new NotImplementedException();
        }

    }
}