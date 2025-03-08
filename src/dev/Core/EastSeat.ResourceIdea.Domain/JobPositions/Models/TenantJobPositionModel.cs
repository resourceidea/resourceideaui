// ----------------------------------------------------------------------------------
// File: TenantJobPositionModel.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\JobPositions\Models\TenantJobPositionModel.cs
// Description: Defines the model for a job position with department information.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.JobPositions.Models
{
    /// <summary>
    /// Tenant job position model with department information.
    /// </summary>
    public class TenantJobPositionModel
    {
        /// <summary>
        /// Gets or sets the job position identifier.
        /// </summary>
        public JobPositionId Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the job position.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the department identifier.
        /// </summary>
        public DepartmentId DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the department name.
        /// </summary>
        public string DepartmentName { get; set; } = string.Empty;
    }
}