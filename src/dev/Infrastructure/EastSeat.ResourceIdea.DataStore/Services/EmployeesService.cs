// ===============================================================================================
// File: EmployeesService.cs
// Path: src/dev/EastSeat.ResourceIdea.DataStore/Services/EmployeeService.cs
// Description: Service class to handle employee operations.
// ===============================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Employee entity operations managing service.
/// </summary>
public class EmployeesService (ResourceIdeaDBContext dbContext, UserManager<ApplicationUser> userManager)
    : IEmployeeService
{
    private readonly ResourceIdeaDBContext _dbContext = dbContext;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    
    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<Employee>> AddAsync(Employee entity, CancellationToken cancellationToken)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        ApplicationUser newApplicationUser = new()
        {
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            UserName = entity.Email,
            ApplicationUserId = entity.ApplicationUserId,
            TenantId = entity.TenantId,
        };

        string temporaryPassword = $"Temp@{Guid.NewGuid().ToString("N")[..8]}";
        IdentityResult result = await _userManager.CreateAsync(newApplicationUser, temporaryPassword);
        if (!result.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ResourceIdeaResponse<Employee>.Failure(ErrorCode.AddApplicationUserFailure);
        }

        EntityEntry<Employee> employeeEntry = await _dbContext.Employees.AddAsync(entity, cancellationToken);
        int changes = await _dbContext.SaveChangesAsync(cancellationToken);
        if (employeeEntry.Entity == null || changes < 1)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ResourceIdeaResponse<Employee>.Failure(ErrorCode.DbInsertFailureOnAddNewEmployee);
        }

        await transaction.CommitAsync(cancellationToken);
        return ResourceIdeaResponse<Employee>.Success(Optional<Employee>.Some(employeeEntry.Entity));
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
