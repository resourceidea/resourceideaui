// ----------------------------------------------------------------------------------
// File: IWorkItemsService.cs
// Path: src\dev\Core\EastSeat.ResourceIdea.Application\Features\WorkItems\Contracts\IWorkItemsService.cs
// Description: Represents the service for managing work items.
// ----------------------------------------------------------------------------------

using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;

namespace EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;

/// <summary>
/// Represents the service for managing work items.
/// </summary>
public interface IWorkItemsService : IDataStoreService<WorkItem>
{
}