// =========================================================================================
// File: UpdateEmployeeCommandHandler.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\Employees\Handlers\UpdateEmployeeCommandHandler.cs
// Description: Mapper for Employee related classes
// =========================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Mapping of the Employee related classes.
/// </summary>
public static class EmployeeMapper
{
    public static ResourceIdeaResponse<PagedListResponse<TModel>> ToResourceIdeaResponse<TEntity, TModel>(
        this ResourceIdeaResponse<PagedListResponse<TEntity>> pagedListResponse)
        where TModel : class
        where TEntity : BaseEntity
    {
        if (!IsSupportedModelType<TModel>())
        {
            return ResourceIdeaResponse<PagedListResponse<TModel>>.UnSupportedOperation();
        }

        if (pagedListResponse.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<TModel>>.Failure(pagedListResponse.Error);
        }

        if (pagedListResponse is null || !pagedListResponse.Content.HasValue)
        {
            return ResourceIdeaResponse<PagedListResponse<TModel>>.NotFound();
        }

        return MapToPagedModelList<TEntity, TModel>(pagedListResponse);
    }

    private static ResourceIdeaResponse<PagedListResponse<TModel>> MapToPagedModelList<TEntity, TModel>(
        ResourceIdeaResponse<PagedListResponse<TEntity>> pagedListResponse)
        where TModel : class
        where TEntity : BaseEntity
    {

        IReadOnlyList<TEntity> sourceItems = pagedListResponse.Content.Value.Items;
        IReadOnlyList<TModel> mappedItems = [.. sourceItems.Select(entity => entity.ToModel<TModel>())];

        var mappedPagedListResponse = new PagedListResponse<TModel>
        {
            Items = mappedItems,
            TotalCount = pagedListResponse.Content.Value.TotalCount,
            CurrentPage = pagedListResponse.Content.Value.CurrentPage,
            PageSize = pagedListResponse.Content.Value.PageSize
        };

        return ResourceIdeaResponse<PagedListResponse<TModel>>.Success(mappedPagedListResponse);
    }

    private static bool IsSupportedModelType<TModel>() where TModel : class
    {
        return typeof(TModel) == typeof(EmployeeModel) ||
               typeof(TModel) == typeof(TenantEmployeeModel);
    }
}
