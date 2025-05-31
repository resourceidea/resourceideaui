using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Departments.Contracts;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service to perform CRUD operations on Department entities.
/// </summary>
public sealed class DepartmentsService(ResourceIdeaDBContext dbContext) : IDepartmentsService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<Department>> AddAsync(Department department, CancellationToken cancellationToken)
    {
        EntityEntry<Department> result = await _dbContext.Departments.AddAsync(department, cancellationToken);
        int changes = await _dbContext.SaveChangesAsync(cancellationToken);
        if (!DepartmentCreatedSuccessfully(result, changes))
        {
            // TODO: Log failure to create department.
            return ResourceIdeaResponse<Department>.Failure(ErrorCode.DbInsertFailureOnCreateDepartment);
        }

        return ResourceIdeaResponse<Department>.Success(Optional<Department>.Some(result.Entity));
    }

    /// <inheritdoc/>
    public Task<ResourceIdeaResponse<Department>> DeleteAsync(Department entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<Department>> GetByIdAsync(BaseSpecification<Department> specification, CancellationToken cancellationToken)
    {
        try
        {
            Department? department = await _dbContext.Departments.FirstOrDefaultAsync(specification.Criteria, cancellationToken);
            if (department == null)
            {
                return ResourceIdeaResponse<Department>.NotFound();
            }

            return ResourceIdeaResponse<Department>.Success(Optional<Department>.Some(department));
        }
        catch (Exception)
        {
            // TODO: Log the exception here if logging is available
            return ResourceIdeaResponse<Department>.Failure(ErrorCode.QueryForDepartmentFailure);
        }
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<PagedListResponse<Department>>> GetPagedListAsync(int page, int size, BaseSpecification<Department>? specification, CancellationToken cancellationToken)
    {
        try
        {
            IQueryable<Department> query = _dbContext.Departments.AsQueryable();

            if (specification != null)
            {
                query = query.Where(specification.Criteria);
            }

            int totalCount = await query.CountAsync(cancellationToken);
            List<Department> items = await query
                                          .Skip((page - 1) * size)
                                          .Take(size)
                                          .OrderBy(d => d.Name)
                                          .ToListAsync(cancellationToken);

            PagedListResponse<Department> pagedList = new()
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = size
            };

            return ResourceIdeaResponse<PagedListResponse<Department>>.Success(Optional<PagedListResponse<Department>>.Some(pagedList));
        }
        catch (Exception)
        {
            // TODO: Log the exception here if logging is available
            return ResourceIdeaResponse<PagedListResponse<Department>>.Failure(ErrorCode.DataStoreCommandFailure);
        }
    }

    /// <inheritdoc/>
    public async Task<ResourceIdeaResponse<Department>> UpdateAsync(
        Department entity,
        CancellationToken cancellationToken)
    {
        try
        {
            Department? department = await _dbContext.Departments
                                                     .FirstOrDefaultAsync(d => d.Id == entity.Id, cancellationToken);
            if (department == null)
            {
                return ResourceIdeaResponse<Department>.NotFound();
            }

            department.Name = entity.Name;
            _dbContext.Departments.Update(department);
            int changes = await _dbContext.SaveChangesAsync(cancellationToken);
            if (changes <= 0)
            {
                // TODO: Log failure to update department.
                return ResourceIdeaResponse<Department>.Failure(ErrorCode.DbUpdateFailureOnUpdateDepartment);
            }

            return ResourceIdeaResponse<Department>.Success(Optional<Department>.Some(department));
        }
        catch (Exception)
        {
            // TODO: Log exception on failure to update department.
            return ResourceIdeaResponse<Department>.Failure(ErrorCode.DbUpdateFailureOnUpdateDepartment);
        }
    }

    private static bool DepartmentCreatedSuccessfully(EntityEntry<Department> result, int changes)
        => result.Entity != null && changes > 0;
}
