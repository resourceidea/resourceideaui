using EastSeat.ResourceIdea.Domain.Common.Entities;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Features.Common.Handlers;

/// <summary>
/// Base handler class providing common functionality for handling responses.
/// </summary>
public class BaseHandler
{
    /// <summary>
    /// Handles the response by converting the entity to a model and returning a new response.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam        >
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <param name="response">The response containing the entity.</param>
    /// <param name="errorCode">The error code to return if the entity is empty.</param>
    /// <param name="entityToModelConverter">The function to convert the entity to a model.</param>
    /// <returns>A new response containing the model or an error code.</returns>
    public static ResourceIdeaResponse<TModel> GetHandlerResponse<TEntity, TModel>(ResourceIdeaResponse<TEntity> response, ErrorCode errorCode)
        where TEntity : BaseEntity
        where TModel : class
    {
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<TModel>.Failure(response.Error);
        }
        if (response.Content.HasValue is false)
        {
            return ResourceIdeaResponse<TModel>.Failure(errorCode);
        }

        return response.Content.Value.ToResourceIdeaResponse<TEntity, TModel>();
    }

    public static ResourceIdeaResponse<IReadOnlyList<TModel>> GetHandlerResponse<TEntity, TModel>(ResourceIdeaResponse<IReadOnlyList<TEntity>> response)
        where TEntity : BaseEntity
        where TModel : class
    {
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<IReadOnlyList<TModel>>.Failure(response.Error);
        }
        if (response.Content.HasValue is false || response.Content.Value.Count == 0)
        {
            return ResourceIdeaResponse<IReadOnlyList<TModel>>.Failure(ErrorCode.NotFound);
        }

        var items = response.Content.Value;
        var models = items.Select(item => item.ToModel<TModel>()).ToList();

        return ResourceIdeaResponse<IReadOnlyList<TModel>>.Success(models);
    }

    public static ResourceIdeaResponse<PagedListResponse<TModel>> GetHandlerResponse<TEntity, TModel>(ResourceIdeaResponse<PagedListResponse<TEntity>> response)
        where TEntity : BaseEntity
        where TModel : class
    {
        if (response.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<TModel>>.Failure(response.Error);
        }
        if (response.Content.HasValue is false || response.Content.Value.TotalCount == 0)
        {
            return ResourceIdeaResponse<PagedListResponse<TModel>>.Failure(ErrorCode.NotFound);
        }

        var items = response.Content.Value.Items;
        PagedListResponse<TModel> pagedListResponse = new()
        {
            Items = items.Select(item => item.ToModel<TModel>()).ToList(),
            TotalCount = response.Content.Value.TotalCount,
            CurrentPage = response.Content.Value.CurrentPage,
            PageSize = response.Content.Value.PageSize
        };

        return ResourceIdeaResponse<PagedListResponse<TModel>>.Success(pagedListResponse);
    }
}
