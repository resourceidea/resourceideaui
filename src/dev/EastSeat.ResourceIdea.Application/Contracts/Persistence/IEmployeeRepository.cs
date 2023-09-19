using EastSeat.ResourceIdea.Domain.Entities;

namespace EastSeat.ResourceIdea.Application.Contracts.Persistence;

/// <summary>
/// Employee repository.
/// </summary>
public interface IEmployeeRepository : IAsyncRepository<Employee>
{
    /// <summary>
    /// Check if user of a given subscription exists.
    /// </summary>
    /// <param name="subscriptionId">Subscription Id.</param>
    /// <param name="userId">User Id.</param>
    /// <returns>True if the employee exists, otherwise False.</returns>
    Task<bool> IsExisitingEmployee(Guid subscriptionId, string userId);
}
