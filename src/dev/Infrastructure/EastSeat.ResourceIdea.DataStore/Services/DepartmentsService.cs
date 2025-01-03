using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service to perform CRUD operations on Department entities.
/// </summary>
public sealed class DepartmentsService : IDepartmentsService
{
    private readonly ResourceIdeaDBContext _dbContext;

    public DepartmentsService(ResourceIdeaDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<Department>> AddAsync(Department department, CancellationToken cancellationToken)
    {
        try
        {
            EntityEntry<Department> result = await _dbContext.Departments.AddAsync(department, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            if (result.Entity is null)
            {
                return ResourceIdeaResponse<Department>.Failure(ErrorCode.EmptyEntityOnCreateDepartment);
            }

            if (result.State != EntityState.Added)
            {
                return ResourceIdeaResponse<Department>.Failure(ErrorCode.DataStoreCommandFailure);
            }

            return ResourceIdeaResponse<Department>.Success(Optional<Department>.Some(result.Entity));
        }
        catch (Exception)
        {
            // TODO: Log the exception here if logging is available
            return ResourceIdeaResponse<Department>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<Department>> DeleteAsync(Department entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<Department>> GetByIdAsync(BaseSpecification<Department> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<PagedListResponse<Department>>> GetPagedListAsync(int page, int size, Optional<BaseSpecification<Department>> specification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<Department>> UpdateAsync(Department entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
