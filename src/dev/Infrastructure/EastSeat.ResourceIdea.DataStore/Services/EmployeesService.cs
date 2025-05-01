// ===============================================================================================
// File: EmployeesService.cs
// Path: src/dev/EastSeat.ResourceIdea.DataStore/Services/EmployeeService.cs
// Description: Service class to handle employee operations.
// ===============================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.DataStore.Identity.Entities;
using EastSeat.ResourceIdea.Domain.Departments.Entities;
using EastSeat.ResourceIdea.Domain.Departments.ValueObjects;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.JobPositions.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Employee entity operations managing service.
/// </summary>
public class EmployeesService(ResourceIdeaDBContext dbContext, UserManager<ApplicationUser> userManager)
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
    public async Task<ResourceIdeaResponse<Employee>> GetByIdAsync(
        BaseSpecification<Employee> specification,
        CancellationToken cancellationToken)
    {
        if (specification is not GetEmployeeIdBySpecification employeeIdSpecification)
        {
            return ResourceIdeaResponse<Employee>.Failure(ErrorCode.FailureOnTenantEmployeesQuery);
        }
        try
        {
            TenantEmployeeModel tenantEmployee = await GetTenantEmployeeByIdAsync(
                employeeIdSpecification.EmployeeId, employeeIdSpecification.TenantId, cancellationToken);

            if (tenantEmployee.IsEmpty)
            {
                return ResourceIdeaResponse<Employee>.Failure(ErrorCode.EmployeeNotFound);
            }

            Employee employee = new()
            {
                EmployeeId = tenantEmployee.EmployeeId,
                JobPositionId = tenantEmployee.JobPositionId,
                EmployeeNumber = tenantEmployee.EmployeeNumber,
                FirstName = tenantEmployee.FirstName ?? string.Empty,
                LastName = tenantEmployee.LastName ?? string.Empty,
                Email = tenantEmployee.Email ?? string.Empty,
                ApplicationUserId = tenantEmployee.ApplicationUserId,
                TenantId = tenantEmployee.TenantId,
                JobPosition = new JobPosition
                {
                    Id = tenantEmployee.JobPositionId,
                    Title = tenantEmployee.JobPositionTitle,
                    Department = new Department
                    {
                        Id = tenantEmployee.DepartmentId,
                        Name = tenantEmployee.DepartmentName ?? string.Empty
                    }
                }
            };

            return ResourceIdeaResponse<Employee>.Success(Optional<Employee>.Some(employee));
        }
        catch (Exception)
        {
            // TODO: Log exception
            return ResourceIdeaResponse<Employee>.Failure(ErrorCode.FailureOnTenantEmployeesQuery);
        }
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<PagedListResponse<Employee>>> GetPagedListAsync(
        int page,
        int size,
        Optional<BaseSpecification<Employee>> specification,
        CancellationToken cancellationToken)
    {
        try
        {
            if (specification.HasValue is false || specification.Value is not TenantEmployeesSpecification)
            {
                return ResourceIdeaResponse<PagedListResponse<Employee>>.Failure(ErrorCode.FailureOnTenantEmployeesQuery);
            }

            Guid? tenantId = GetTenantIdFromSpecification(specification);
            int tenantEmployeesCount = await GetTotalItemCountAsync(tenantId, cancellationToken);
            IReadOnlyList<TenantEmployeeModel> queryResults = await QueryForTenantEmployeesAsync(page, size, tenantId, cancellationToken);
            IReadOnlyList<Employee> employees = MapToEmployeesList(queryResults);
            PagedListResponse<Employee> pagedList = GetPagedList(page, size, tenantEmployeesCount, employees);

            return ResourceIdeaResponse<PagedListResponse<Employee>>.Success(Optional<PagedListResponse<Employee>>.Some(pagedList));
        }
        catch (Exception)
        {
            // TODO: Log exception.
            return ResourceIdeaResponse<PagedListResponse<Employee>>.Failure(ErrorCode.FailureOnTenantEmployeesQuery);
        }
    }

    private static PagedListResponse<Employee> GetPagedList(int page, int size, int totalCount, IReadOnlyList<Employee> employees) =>
        new()
        {
            Items = employees,
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = size
        };

    private static Guid? GetTenantIdFromSpecification(Optional<BaseSpecification<Employee>> specification)
    {
        TenantEmployeesSpecification? employeeSpecification = specification.Value as TenantEmployeesSpecification;
        TenantId tenantId = employeeSpecification?.TenantId ?? TenantId.Empty;
        return tenantId.Value;
    }

    private async Task<int> GetTotalItemCountAsync(Guid? tenantId, CancellationToken cancellationToken)
    {
        string countSql = "SELECT COUNT(*) FROM Employees e WHERE e.TenantId = @TenantId";
        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        using var command = new SqlCommand(countSql, connection);
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        object result = await command.ExecuteScalarAsync(cancellationToken);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    private async Task<TenantEmployeeModel> GetTenantEmployeeByIdAsync(
        EmployeeId employeeId,
        TenantId tenantId,
        CancellationToken cancellationToken)
    {
        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        string sql = @"SELECT TOP 1 e.EmployeeId, e.JobPositionId, e.EmployeeNumber, e.ApplicationUserId, e.TenantId,
                          u.FirstName, u.LastName, u.Email,
                          jp.Title as 'JobPositionTitle', 
                          d.Id as 'DepartmentId', d.Name as 'DepartmentName'
                   FROM [dbo].[Employees] e
                   JOIN [Identity].[ApplicationUsers] u ON e.ApplicationUserId = u.ApplicationUserId
                   LEFT JOIN [dbo].[JobPositions] jp ON e.JobPositionId = jp.Id
                   LEFT JOIN [dbo].[Departments] d ON jp.DepartmentId = d.Id
                   WHERE e.TenantId = @TenantId AND e.EmployeeId = @EmployeeId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@EmployeeId", employeeId.Value));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId.Value));

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            return new TenantEmployeeModel
            {
                EmployeeId = EmployeeId.Create(reader.GetString(reader.GetOrdinal("EmployeeId"))),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                JobPositionId = JobPositionId.Create(reader.GetString(reader.GetOrdinal("JobPositionId"))),
                JobPositionTitle = reader.GetString(reader.GetOrdinal("JobPositionTitle")),
                DepartmentId = DepartmentId.Create(reader.GetString(reader.GetOrdinal("DepartmentId"))),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                ApplicationUserId = ApplicationUserId.Create(reader.GetString(reader.GetOrdinal("ApplicationUserId"))),
                TenantId = TenantId.Create(reader.GetString(reader.GetOrdinal("TenantId")))
            };
        }

        return TenantEmployeeModel.Empty;
    }

    private async Task<List<TenantEmployeeModel>> QueryForTenantEmployeesAsync(
        int page,
        int size,
        Guid? tenantId,
        CancellationToken cancellationToken)
    {
        List<TenantEmployeeModel> queryResults = [];

        using var connection = new SqlConnection(_dbContext.Database.GetConnectionString());
        await connection.OpenAsync(cancellationToken);

        string sql = @"SELECT e.EmployeeId, e.JobPositionId, e.EmployeeNumber,
                              u.FirstName, u.LastName, u.Email,
                              jp.Title as 'JobPositionTitle', 
                              d.Id as 'DepartmentId', d.Name as 'DepartmentName'
                       FROM [dbo].[Employees] e
                       JOIN [Identity].[ApplicationUsers] u ON e.ApplicationUserId = u.ApplicationUserId
                       LEFT JOIN [dbo].[JobPositions] jp ON e.JobPositionId = jp.Id
                       LEFT JOIN [dbo].[Departments] d ON jp.DepartmentId = d.Id
                       WHERE e.TenantId = @TenantId
                       ORDER BY u.FirstName
                       OFFSET @Offset ROWS
                       FETCH NEXT @PageSize ROWS ONLY";

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
            var tenantEmployee = new TenantEmployeeModel
            {
                EmployeeId = EmployeeId.Create(reader.GetString(reader.GetOrdinal("EmployeeId"))),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                JobPositionId = JobPositionId.Create(reader.GetString(reader.GetOrdinal("JobPositionId"))),
                JobPositionTitle = reader.GetString(reader.GetOrdinal("JobPositionTitle")),
                DepartmentId = DepartmentId.Create(reader.GetString(reader.GetOrdinal("DepartmentId"))),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName"))
            };

            queryResults.Add(tenantEmployee);
        }

        return queryResults;
    }

    private static IReadOnlyList<Employee> MapToEmployeesList(IReadOnlyList<TenantEmployeeModel> queryResults)
    {
        return [.. queryResults
                .Select(e => new Employee
                {
                    EmployeeId = e.EmployeeId,
                    JobPositionId = e.JobPositionId,
                    EmployeeNumber = e.EmployeeNumber,
                    FirstName = e.FirstName ?? string.Empty,
                    LastName = e.LastName ?? string.Empty,
                    Email = e.Email ?? string.Empty,
                    JobPosition = new JobPosition
                    {
                        Id = e.JobPositionId,
                        Title = e.JobPositionTitle,
                        Department = new Department
                        {
                            Id = e.DepartmentId,
                            Name = e.DepartmentName ?? string.Empty
                        }
                    }
                })];
    }

    /// <inheritdoc />
    public async Task<ResourceIdeaResponse<Employee>> UpdateAsync(Employee entity, CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            ApplicationUser? applicationUser = await _userManager.FindByIdAsync(entity.ApplicationUserId.ToString());
            if (applicationUser == null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ResourceIdeaResponse<Employee>.Failure(ErrorCode.ApplicationUserNotFound);
            }
            applicationUser.FirstName = entity.FirstName;
            applicationUser.LastName = entity.LastName;
            await _userManager.UpdateAsync(applicationUser);

            Employee? existingEmployee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == entity.EmployeeId, cancellationToken);
            if (existingEmployee == null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ResourceIdeaResponse<Employee>.Failure(ErrorCode.EmployeeNotFound);
            }
            existingEmployee.JobPositionId = entity.JobPositionId;
            existingEmployee.FirstName = entity.FirstName;
            existingEmployee.LastName = entity.LastName;
            _dbContext.Employees.Update(existingEmployee);

            int changes = await _dbContext.SaveChangesAsync(cancellationToken);
            if (changes < 1)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ResourceIdeaResponse<Employee>.Failure(ErrorCode.DbUpdateFailureOnUpdateEmployee);
            }

            await transaction.CommitAsync(cancellationToken);
            return ResourceIdeaResponse<Employee>.Success(Optional<Employee>.Some(existingEmployee));
        }
        catch (DbUpdateException)
        {
            // Handles database update conflicts, constraint violations
            // TODO: Log specific database update exception
            return ResourceIdeaResponse<Employee>.Failure(ErrorCode.DbUpdateFailureOnUpdateEmployee);
        }
        catch (SqlException)
        {
            // Handles SQL Server-specific errors like timeouts, connection issues
            // TODO: Log SQL exception details
            return ResourceIdeaResponse<Employee>.Failure(ErrorCode.DbUpdateFailureOnUpdateEmployee);
        }
    }
}
