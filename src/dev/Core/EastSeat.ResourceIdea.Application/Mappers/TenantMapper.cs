using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Tenants.Commands;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Tenants.Entities;
using EastSeat.ResourceIdea.Domain.Tenants.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Mappers;

/// <summary>
/// Mapper for Tenant entity and models.
/// </summary>
public static class TenantMapper
{
    /// <summary>
    /// Converts a Tenant entity to a specified model type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to convert to.</typeparam>
    /// <param name="tenant">The tenant entity to convert.</param>
    /// <returns>The converted model of type <typeparamref name="TModel"/>.</returns>
    /// <exception cref="InvalidCastException">Thrown when the conversion to the specified model type fails.</exception>
    /// <exception cref="NotImplementedException">Thrown when the specified model type is not supported.</exception>
    public static TModel ToModel<TModel>(this Tenant tenant) where TModel : class
    {
        return tenant switch
        {
            Tenant entity when typeof(TModel) == typeof(TenantModel) => ToTenantModel(entity) as TModel ?? throw new InvalidCastException(),
            _ => throw new NotSupportedException($"Mapping of type {typeof(TModel).Name} is not supported")
        };
    }

    /// <summary>
    /// Maps <see cref="TenantModel"/> to <see cref="Tenant"/>
    /// </summary>
    /// <param name="model">The <see cref="TenantModel"/> to convert.</param>
    /// <returns>The converted <see cref="Tenant"/> entity.</returns>
    public static Tenant ToEntity(this TenantModel model)
    {
        return new Tenant
        {
            TenantId = model.TenantId.Value,
            Organization = model.Organization
        };
    }

    /// <summary>
    /// Maps <see cref="CreateTenantCommand"/> to <see cref="Tenant"/>.
    /// </summary>
    /// <param name="command">The <see cref="CreateTenantCommand"/> to convert.</param>
    /// <returns>The converted <see cref="Tenant"/> entity.</returns>
    public static Tenant ToEntity(this CreateTenantCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(command.Organization);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Organization);

        return new Tenant
        {
            TenantId = Guid.NewGuid(),
            Organization = command.Organization
        };
    }

    /// <summary>
    /// Maps <see cref="UpdateTenantCommand"/> to <see cref="Tenant"/>.
    /// </summary>
    /// <param name="command">The <see cref="UpdateTenantCommand"/> to convert.</param>
    /// <returns>The converted <see cref="Tenant"/> entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the organization is null, empty, or whitespace.</exception>
    public static Tenant ToEntity(this UpdateTenantCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrEmpty(command.Organization);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Organization);
        return new Tenant
        {
            Organization = command.Organization
        };
    }

    /// <summary>
    /// Converts a Tenant entity to a ResourceIdeaResponse containing a TenantModel.
    /// </summary>
    /// <param name="tenant">The tenant entity to convert.</param>
    /// <returns>A ResourceIdeaResponse containing the converted TenantModel.</returns>
    public static ResourceIdeaResponse<TenantModel> ToResourceIdeaResponse(this Tenant tenant)
    {
        return ResourceIdeaResponse<TenantModel>.Success(ToTenantModel(tenant));
    }

    /// <summary>
    /// Converts a PagedListResponse of Tenant entities to a ResourceIdeaResponse containing a PagedListResponse of TenantModels.
    /// </summary>
    /// <param name="tenants">The PagedListResponse of Tenant entities to convert.</param>
    /// <returns>A ResourceIdeaResponse containing the converted PagedListResponse of TenantModels.</returns>
    public static ResourceIdeaResponse<PagedListResponse<TenantModel>> ToResourceIdeaResponse(this PagedListResponse<Tenant> tenants)
    {
        return ResourceIdeaResponse<PagedListResponse<TenantModel>>.Success(ToModelPagedListResponse(tenants));
    }

    /// <summary>
    /// Converts a PagedListResponse of Tenant entities to a PagedListResponse of TenantModels.
    /// </summary>
    /// <param name="tenants">The PagedListResponse of Tenant entities to convert.</param>
    /// <returns>The converted PagedListResponse of TenantModels.</returns>
    private static PagedListResponse<TenantModel> ToModelPagedListResponse(PagedListResponse<Tenant> tenants)
    {
        return new()
        {
            PageSize = tenants.PageSize,
            CurrentPage = tenants.CurrentPage,
            TotalCount = tenants.TotalCount,
            Items = [.. tenants.Items.Select(ToTenantModel)]
        };
    }

    /// <summary>
    /// Converts a Tenant entity to a TenantModel.
    /// </summary>
    /// <param name="tenant">The tenant entity to convert.</param>
    /// <returns>The converted TenantModel.</returns>
    private static TenantModel ToTenantModel(Tenant tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);
        tenant.Organization.ThrowIfNullOrEmptyOrWhiteSpace();

        return new TenantModel
        {
            TenantId = TenantId.Create(tenant.TenantId),
            Organization = tenant.Organization
        };
    }
}
