using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace EastSeat.ResourceIdea.Persistence.Repositories;

/// <summary>
/// Repository for the employee records.
/// </summary>
public class EmployeeRepository(ResourceIdeaDbContext dbContext) : BaseRepository<Employee>(dbContext), IEmployeeRepository
{

    /// <inheritdoc/>
    public async Task<bool> IsExisitingEmployee(Guid subscriptionId, string userId)
    {
        return await dbContext.Employees.AnyAsync(employee => employee.SubscriptionId == subscriptionId && employee.UserId == userId);
    }
}
