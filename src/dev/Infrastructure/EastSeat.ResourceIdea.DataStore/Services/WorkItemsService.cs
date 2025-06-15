// =============================================================================================================
// File: WorkItemsService.cs
// Path: src\dev\Infrastructure\EastSeat.ResourceIdea.DataStore\Services\WorkItemsService.cs
// Description: This file contains the WorkItemsService class implementation.
// =============================================================================================================

using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.DataStore.Services.Common;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;

namespace EastSeat.ResourceIdea.DataStore.Services;

/// <summary>
/// Service for managing work items in the data store.
/// </summary>
/// <param name="dbContext">The database context.</param>
public class WorkItemsService(ResourceIdeaDBContext dbContext) : DataStoreService<WorkItem>(dbContext), IWorkItemsService
{
}