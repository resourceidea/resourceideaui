// ===============================================================================================
// File: EmployeesService.cs
// Path: src/dev/EastSeat.ResourceIdea.DataStore/Services/EmployeeService.cs
// Description: Service class to handle employee operations.
// ===============================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Employee entity operations managing service.
/// </summary>
public class EmployeesService (ResourceIdeaDBContext dbContext) : IEmployeeService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<Employee>> AddAsync(Employee entity, CancellationToken cancellationToken)
    {
        EntityEntry<Employee> result = await _dbContext.Employees.AddAsync(entity, cancellationToken);
        int changes = await _dbContext.SaveChangesAsync(cancellationToken);
        if (result.Entity != null && changes > 0)
        {
            return ResourceIdeaResponse<Employee>.Success(Optional<Employee>.Some(result.Entity));
        }

        return ResourceIdeaResponse<Employee>.Failure(ErrorCode.DbInsertFailureOnAddNewEmployee);
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<Employee>> DeleteAsync(Employee entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<Employee>> GetByIdAsync(
        BaseSpecification<Employee> specification,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<PagedListResponse<Employee>>> GetPagedListAsync(
        int page,
        int size,
        Optional<BaseSpecification<Employee>> specification,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task<ResourceIdeaResponse<Employee>> UpdateAsync(Employee entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
