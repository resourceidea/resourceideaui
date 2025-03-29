// ==================================================================================================
// File: IEmployeeService.cs
// Path: src/dev/Core/EastSeat.ResourceIdea.Application/Features/Employee/Contracts/IEmployeeService
// Description: Employee operations handling service interface.
// ==================================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.Employees.Entities;

namespace EastSeat.ResourceIdea.Application.Features.Employees.Contracts;

/// <summary>
/// Employee operations handling service interface.
/// </summary>
public interface IEmployeeService : IDataStoreService<Employee>
{
}
