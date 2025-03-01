// ----------------------------------------------------------------------------------
// File: JobPositionMapper.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Mappers\JobPositionMapper.cs
// Description: Provides extension methods for mapping JobPosition entities to models.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Provides extension methods for mapping JobPosition entities to models.
/// </summary>
public static class JobPositionMapper
{
    /// <summary>
    /// Maps a JobPosition entity to a ResourceIdeaResponse of JobPositionModel.
    /// </summary>
    /// <param name="jobPosition">The job position entity to map.</param>
    /// <returns>The mapped ResourceIdeaResponse of JobPositionModel.</returns>
    public static ResourceIdeaResponse<JobPositionModel> ToResourceIdeaResponse(this JobPosition jobPosition)
    {
        if (jobPosition == null)
        {
            return ResourceIdeaResponse<JobPositionModel>.NotFound();
        }

        var model = jobPosition.ToModel<JobPositionModel>();
        return ResourceIdeaResponse<JobPositionModel>.Success(Optional<JobPositionModel>.Some(model));
    }
}