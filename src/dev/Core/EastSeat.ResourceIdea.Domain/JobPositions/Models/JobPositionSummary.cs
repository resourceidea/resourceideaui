// ================================================================================
// File: JobPositionSummary.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Domain\JobPositions\Models\JobPositionSummary.cs
// Description: Model for job position summary.
// This model is used to represent a summary of a job position, including its ID, title, and employee count.
// ================================================================================

using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.JobPositions.Models;

/// <summary>
/// Model for job position summary.
/// This model is used to represent a summary of a job position, including its ID, title, and employee count.
/// </summary>
public class JobPositionSummary
{
    public JobPositionId JobPositionId { get; set; }
    public string? Title { get; set; }
    public int EmployeeCount { get; set; }

    public int TotalCount { get; set; }
}
