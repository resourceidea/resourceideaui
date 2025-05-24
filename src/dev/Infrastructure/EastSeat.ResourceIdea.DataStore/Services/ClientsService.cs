// ===========================================================================================
// File: ClientsService.cs
// Path: src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\Services\ClientsService.cs
// Description: Service for managing clients.
// ===========================================================================================

using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for managing clients.
/// </summary>
public sealed class ClientsService(ResourceIdeaDBContext dbContext) : IClientsService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<Client>> AddAsync(Client entity, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Clients.AddAsync(entity, cancellationToken);
            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return ResourceIdeaResponse<Client>.Success(entity);
            }

            return ResourceIdeaResponse<Client>.Failure(ErrorCode.DbInsertFailureOnAddClient);
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException?.Message.Contains("duplicate") == true ||
                dbEx.InnerException?.Message.Contains("unique constraint") == true)
            {
                return ResourceIdeaResponse<Client>.Failure(ErrorCode.ClientAlreadyExists);
            }

            return ResourceIdeaResponse<Client>.Failure(ErrorCode.DatabaseError);
        }
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<Client>> DeleteAsync(Client entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<Client>> GetByIdAsync(BaseSpecification<Client> specification, CancellationToken cancellationToken)
    {
        Client? client = await _dbContext.Clients.AsNoTracking().FirstOrDefaultAsync(specification.Criteria, cancellationToken);
        if (client == null)
        {
            return ResourceIdeaResponse<Client>.NotFound();
        }

        return ResourceIdeaResponse<Client>.Success(client);
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<PagedListResponse<Client>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<Client>> specification, CancellationToken cancellationToken)
    {
        if (specification.HasValue is false || specification.Value is not TenantClientsSpecification)
        {
            return ResourceIdeaResponse<PagedListResponse<Client>>.Failure(ErrorCode.FailureOnTenantClientsSpecification);
        }

        Guid? tenantId = GetTenantIdFromSpecification(specification);
        int tenantClientsCount = await GetTotalClientsCountAsync(tenantId, cancellationToken);
        IReadOnlyList<TenantClientModel> queryResult = await QueryForTenantClientsAsync(page, size, tenantId, cancellationToken);
        IReadOnlyList<Client> clients = MapToClientsList(queryResult);
        var pagedListResponse = GetPagedClientsList(page, size, tenantClientsCount, clients);

        return ResourceIdeaResponse<PagedListResponse<Client>>.Success(pagedListResponse);
    }

    private static PagedListResponse<Client> GetPagedClientsList(int page, int size, int tenantClientsCount, IReadOnlyList<Client> clients) => new()
    {
        Items = clients,
        TotalCount = tenantClientsCount,
        CurrentPage = page,
        PageSize = size,
    };

    private static IReadOnlyList<Client> MapToClientsList(IReadOnlyList<TenantClientModel> queryResult)
    {
        return [.. queryResult.Select(clientModel => new Client
        {
            Id = clientModel.ClientId,
            Name = clientModel.Name,
            Address = clientModel.Address
        })];
    }

    private async Task<IReadOnlyList<TenantClientModel>> QueryForTenantClientsAsync(
        int page,
        int size,
        Guid? tenantId,
        CancellationToken cancellationToken)
    {
        List<TenantClientModel> queryResults = [];
        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        string sql = @"
            SELECT c.Id, c.Name, c.Address_Building, c.Address_Street, c.Address_City
            FROM [dbo].[Clients] c
            WHERE c.TenantId = @TenantId
            ORDER BY c.Name
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var sqlParameters = new List<object>
            {
                new SqlParameter("@Offset", (page - 1) * size),
                new SqlParameter("@PageSize", size),
                new SqlParameter("@TenantId", tenantId)
            };

        using var command = new SqlCommand(sql, connection);
        foreach (var parameter in sqlParameters)
        {
            command.Parameters.Add(parameter);
        }

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            ClientId clientId = ClientId.Create(reader.GetString(reader.GetOrdinal("Id")));
            string name = reader.GetString(reader.GetOrdinal("Name"));
            Address address = Address.Create(
                building: reader.GetString(reader.GetOrdinal("Address_Building")),
                street: reader.GetString(reader.GetOrdinal("Address_Street")),
                city: reader.GetString(reader.GetOrdinal("Address_City")));
            TenantClientModel tenantClient = new(clientId, address, name);
            queryResults.Add(tenantClient);
        }

        return queryResults;
    }

    private async Task<int> GetTotalClientsCountAsync(Guid? tenantId, CancellationToken cancellationToken)
    {
        string countSql = "SELECT COUNT(*) FROM [dbo].[Clients] c WHERE c.TenantId = @TenantId";
        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(countSql, connection);
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        object result = await command.ExecuteScalarAsync(cancellationToken);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    private static Guid? GetTenantIdFromSpecification(Optional<BaseSpecification<Client>> specification)
    {
        TenantClientsSpecification? tenantClientsSpecification = specification.Value as TenantClientsSpecification;
        TenantId tenantId = tenantClientsSpecification?.TenantId ?? TenantId.Empty;
        return tenantId.Value;
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<Client>> UpdateAsync(Client entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
