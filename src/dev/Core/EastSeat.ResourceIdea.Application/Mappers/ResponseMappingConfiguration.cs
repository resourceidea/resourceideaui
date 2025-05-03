// ----------------------------------------------------------------------------------
// File: ClientMapper.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Mappers\ClientMapper.cs
// Description: Provides extension methods for mapping between client entities and models.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Provides extension methods for mapping between client entities and models.
/// </summary>
public static class ResponseMappingConfiguration
{
    /// <summary>
    /// Maps a <see cref="ResourceIdeaResponse{PagedListResponse{TEntity}}"/> entity
    /// to a <see cref="ResourceIdeaResponse{PagedListResponse{TModel}}"/>.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="pagedListResponse"></param>
    /// <returns></returns>
    public static ResourceIdeaResponse<PagedListResponse<TModel>> ToResourceIdeaResponse<TEntity, TModel>(
        this ResourceIdeaResponse<PagedListResponse<TEntity>> pagedListResponse)
        where TModel : class
        where TEntity : BaseEntity
    {
        if (!CanMapEntityToModel<TEntity, TModel>() || !IsSupportedModelType<TModel>())
        {
            return ResourceIdeaResponse<PagedListResponse<TModel>>.UnSupportedOperation();
        }

        if (pagedListResponse is null || pagedListResponse.IsFailure || !pagedListResponse.Content.HasValue)
        {
            return pagedListResponse is null || !pagedListResponse.Content.HasValue
                ? ResourceIdeaResponse<PagedListResponse<TModel>>.NotFound()
                : ResourceIdeaResponse<PagedListResponse<TModel>>.Failure(pagedListResponse.Error);
        }

        IReadOnlyList<TModel> mappedItems = [.. pagedListResponse.Content.Value.Items.Select(entity => entity.ToModel<TModel>())];

        var mappedPagedListResponse = new PagedListResponse<TModel>
        {
            Items = mappedItems,
            TotalCount = pagedListResponse.Content.Value.TotalCount,
            CurrentPage = pagedListResponse.Content.Value.CurrentPage,
            PageSize = pagedListResponse.Content.Value.PageSize
        };

        return ResourceIdeaResponse<PagedListResponse<TModel>>.Success(mappedPagedListResponse);
    }

    private static bool IsSupportedModelType<TModel>()
    {
        return typeof(TModel) == typeof(TenantClientModel)
            || typeof(TModel) == typeof(ClientModel)
            || typeof(TModel) == typeof(TenantEmployeeModel)
            || typeof(TModel) == typeof(EmployeeModel);
    }

    public static bool CanMapEntityToModel<TEntity, TModel>()
    {
        return (typeof(TEntity).Name, typeof(TModel).Name) switch
        {
            ("Client", "TenantClientModel") => true,
            ("Client", "ClientModel") => true,
            ("Employee", "TenantEmployeeModel") => true,
            ("Employee", "EmployeeModel") => true,
            _ => false
        };
    }
}
