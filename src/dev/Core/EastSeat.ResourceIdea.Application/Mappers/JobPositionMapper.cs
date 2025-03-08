// ----------------------------------------------------------------------------------
// File: JobPositionMapper.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Mappers\JobPositionMapper.cs
// Description: Provides extension methods for mapping JobPosition entities to models.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.Models;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
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

    public static ResourceIdeaResponse<PagedListResponse<TModel>> ToResourceIdeaResponse<TModel>(
        this ResourceIdeaResponse<PagedListResponse<JobPosition>> pagedListResponse)
        where TModel : class
    {
        return typeof(TModel) switch
        {
            var t when t == typeof(JobPositionModel) => (ResourceIdeaResponse<PagedListResponse<TModel>>)(object)MapToPagedJobPositionModelsList(pagedListResponse),
            var t when t == typeof(TenantJobPositionModel) => (ResourceIdeaResponse<PagedListResponse<TModel>>)(object)MapToPagedTenantJobPositionModelsList(pagedListResponse),
            _ => throw new InvalidOperationException($"Mapping for {typeof(TModel).Name} is not configured."),
        };
    }

    private static ResourceIdeaResponse<PagedListResponse<JobPositionModel>> MapToPagedJobPositionModelsList(
        ResourceIdeaResponse<PagedListResponse<JobPosition>> pagedListResponse)
    {
        if (pagedListResponse == null)
        {
            return ResourceIdeaResponse<PagedListResponse<JobPositionModel>>.NotFound();
        }

        var mappedItems = pagedListResponse.Content.Value.Items
            .Select(jobPosition => jobPosition.ToModel<JobPositionModel>())
            .ToList();
        var mappedPagedListResponse = new PagedListResponse<JobPositionModel>
        {
            Items = mappedItems,
            TotalCount = pagedListResponse.Content.Value.TotalCount,
            CurrentPage = pagedListResponse.Content.Value.CurrentPage,
            PageSize = pagedListResponse.Content.Value.PageSize
        };

        return ResourceIdeaResponse<PagedListResponse<JobPositionModel>>.Success(mappedPagedListResponse);
    }

    private static ResourceIdeaResponse<PagedListResponse<TenantJobPositionModel>> MapToPagedTenantJobPositionModelsList(
        ResourceIdeaResponse<PagedListResponse<JobPosition>> pagedListResponse)
    {
        if (pagedListResponse == null)
        {
            return ResourceIdeaResponse<PagedListResponse<TenantJobPositionModel>>.NotFound();
        }

        var mappedItems = pagedListResponse.Content.Value.Items
            .Select(jobPosition => jobPosition.ToModel<TenantJobPositionModel>())
            .ToList();
        var mappedPagedListResponse = new PagedListResponse<TenantJobPositionModel>
        {
            Items = mappedItems,
            TotalCount = pagedListResponse.Content.Value.TotalCount,
            CurrentPage = pagedListResponse.Content.Value.CurrentPage,
            PageSize = pagedListResponse.Content.Value.PageSize
        };

        return ResourceIdeaResponse<PagedListResponse<TenantJobPositionModel>>.Success(mappedPagedListResponse);
    }
}