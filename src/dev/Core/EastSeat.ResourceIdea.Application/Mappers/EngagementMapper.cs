﻿// ----------------------------------------------------------------------------------
// File: EngagementMapper.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Mappers\EngagementMapper.cs
// Description: Provides extension methods for mapping Engagement entities to models.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Provides extension methods for mapping Engagement entities to models.
/// </summary>
public static class EngagementMapper
{
    /// <summary>
    /// Maps an Engagement entity to a model of type TModel.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to map to.</typeparam>
    /// <param name="engagement">The engagement entity to map.</param>
    /// <returns>The mapped model of type TModel.</returns>
    /// <exception cref="InvalidCastException">Thrown when the cast to TModel fails.</exception>
    /// <exception cref="NotSupportedException">Thrown when the mapping to the specified type is not supported.</exception>
    public static TModel ToModel<TModel>(this Engagement engagement) where TModel : class
    {
        return engagement switch
        {
            Engagement e when typeof(TModel) == typeof(EngagementModel) => ToEngagementModel(e) as TModel ?? throw new InvalidCastException(),
            _ => throw new NotSupportedException($"Mapping of type {typeof(TModel).Name} is not supported")
        };
    }    /// <summary>
         /// Maps an EngagementModel to an Engagement entity.
         /// </summary>
         /// <param name="model">The engagement model to map.</param>
         /// <returns>The mapped Engagement entity.</returns>
    public static Engagement ToEntity(this EngagementModel model)
    {
        return new Engagement
        {
            Id = model.Id,
            Title = model.Title,
            ClientId = model.ClientId,
            TenantId = model.TenantId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            EngagementStatus = model.Status,
            Description = model.Description,
            ManagerId = model.ManagerId,
            PartnerId = model.PartnerId
        };
    }    /// <summary>
         /// Maps a CreateEngagementCommand to an Engagement entity.
         /// </summary>
         /// <param name="command">The create engagement command to map.</param>
         /// <returns>The mapped Engagement entity.</returns>
    public static Engagement ToEntity(this CreateEngagementCommand command)
    {
        return new Engagement
        {
            Id = EngagementId.Create(Guid.NewGuid()),
            Title = command.Title,
            ClientId = command.ClientId,
            EngagementStatus = command.Status,
            Description = command.Description ?? string.Empty,
            EndDate = command.DueDate,
            TenantId = command.TenantId
            // Note: ManagerId and PartnerId are not set during creation as they are typically assigned later
        };
    }    /// <summary>
         /// Maps an UpdateEngagementCommand to an Engagement entity.
         /// </summary>
         /// <param name="command">The update engagement command to map.</param>
         /// <returns>The mapped Engagement entity.</returns>
    public static Engagement ToEntity(this UpdateEngagementCommand command)
    {
        return new Engagement
        {
            Id = command.EngagementId,
            ClientId = command.ClientId,
            EngagementStatus = command.Status,
            StartDate = command.StartDate == DateTimeOffset.MinValue
                ? null
                : command.StartDate,
            EndDate = command.EndDate == DateTimeOffset.MinValue
                ? null
                : command.EndDate,
            Description = command.Description ?? string.Empty
            // Note: Title, ManagerId, and PartnerId are not available in UpdateEngagementCommand and would need to be updated through separate operations
        };
    }

    /// <summary>
    /// Maps an Engagement entity to a ResourceIdeaResponse of EngagementModel.
    /// </summary>
    /// <param name="engagement">The engagement entity to map.</param>
    /// <returns>The mapped ResourceIdeaResponse of EngagementModel.</returns>
    public static ResourceIdeaResponse<EngagementModel> ToResourceIdeaResponse(this Engagement engagement)
    {
        return ResourceIdeaResponse<EngagementModel>.Success(ToEngagementModel(engagement));
    }

    /// <summary>
    /// Maps a PagedListResponse of Engagement entities to a ResourceIdeaResponse of PagedListResponse of EngagementModel.
    /// </summary>
    /// <param name="engagements">The paged list of engagement entities to map.</param>
    /// <returns>The mapped ResourceIdeaResponse of PagedListResponse of EngagementModel.</returns>
    public static ResourceIdeaResponse<PagedListResponse<EngagementModel>> ToResourceIdeaResponse(this PagedListResponse<Engagement> engagements)
    {
        return ResourceIdeaResponse<PagedListResponse<EngagementModel>>.Success(ToModelPagedListResponse(engagements));
    }    /// <summary>
         /// Maps an Engagement entity to an EngagementModel.
         /// </summary>
         /// <param name="engagement">The engagement entity to map.</param>
         /// <returns>The mapped EngagementModel.</returns>
    private static EngagementModel ToEngagementModel(Engagement engagement)
    {
        return new EngagementModel
        {
            Id = engagement.Id,
            Title = engagement.Title,
            ClientId = engagement.ClientId,
            TenantId = engagement.TenantId,
            StartDate = engagement.StartDate,
            EndDate = engagement.EndDate,
            Status = engagement.EngagementStatus,
            Description = engagement.Description ?? string.Empty,
            ClientName = engagement.Client?.Name ?? string.Empty,
            ManagerId = engagement.ManagerId,
            PartnerId = engagement.PartnerId
        };
    }

    /// <summary>
    /// Maps a PagedListResponse of Engagement entities to a PagedListResponse of EngagementModel.
    /// </summary>
    /// <param name="engagements">The paged list of engagement entities to map.</param>
    /// <returns>The mapped PagedListResponse of EngagementModel.</returns>
    private static PagedListResponse<EngagementModel> ToModelPagedListResponse(PagedListResponse<Engagement> engagements)
    {
        return new()
        {
            Items = [.. engagements.Items.Select(ToEngagementModel)],
            CurrentPage = engagements.CurrentPage,
            PageSize = engagements.PageSize,
            TotalCount = engagements.TotalCount
        };
    }
}
