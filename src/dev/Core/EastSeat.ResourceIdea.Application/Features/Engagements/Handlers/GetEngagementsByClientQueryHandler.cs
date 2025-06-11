using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Engagements.Contracts;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Types;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public class GetEngagementsByClientQueryHandler(IEngagementsService engagementsService)
    : IRequestHandler<GetEngagementsByClientQuery, ResourceIdeaResponse<PagedListResponse<EngagementModel>>>
{
    private readonly IEngagementsService _engagementsService = engagementsService;

    public async Task<ResourceIdeaResponse<PagedListResponse<EngagementModel>>> Handle(GetEngagementsByClientQuery request, CancellationToken cancellationToken)
    {
        var getEngagementsByClientSpecification = new GetEngagementsByClientSpecification(
            request.ClientId,
            request.TenantId,
            request.SearchTerm);

        // For now, we'll get all matching engagements and handle sorting/paging in memory
        // In a production system, this should be handled at the database level
        var result = await _engagementsService.GetPagedListAsync(
            1, 
            int.MaxValue, // Get all records for now to handle sorting
            getEngagementsByClientSpecification,
            cancellationToken);
            
        if (result.IsFailure)
        {
            return ResourceIdeaResponse<PagedListResponse<EngagementModel>>.Failure(result.Error);
        }

        if (result.Content.HasValue is false)
        {
            return ResourceIdeaResponse<PagedListResponse<EngagementModel>>.NotFound();
        }

        var engagements = result.Content.Value.ToResourceIdeaResponse().Content.Value;
        
        // Apply sorting
        var sortedEngagements = ApplySorting(engagements.Items, request.SortField, request.SortDirection);
        
        // Apply paging
        var pagedResult = ApplyPaging(sortedEngagements, request.PageNumber, request.PageSize);
        
        return ResourceIdeaResponse<PagedListResponse<EngagementModel>>.Success(pagedResult);
    }

    private IEnumerable<EngagementModel> ApplySorting(IEnumerable<EngagementModel> engagements, string? sortField, string? sortDirection)
    {
        if (string.IsNullOrEmpty(sortField))
            return engagements.OrderBy(e => e.Description); // Default sort

        var isDescending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return sortField.ToLowerInvariant() switch
        {
            "description" => isDescending ? engagements.OrderByDescending(e => e.Description) : engagements.OrderBy(e => e.Description),
            "status" => isDescending ? engagements.OrderByDescending(e => e.Status) : engagements.OrderBy(e => e.Status),
            "commencementdate" => isDescending ? engagements.OrderByDescending(e => e.CommencementDate) : engagements.OrderBy(e => e.CommencementDate),
            "completiondate" => isDescending ? engagements.OrderByDescending(e => e.CompletionDate) : engagements.OrderBy(e => e.CompletionDate),
            _ => engagements.OrderBy(e => e.Description)
        };
    }

    private PagedListResponse<EngagementModel> ApplyPaging(IEnumerable<EngagementModel> engagements, int pageNumber, int pageSize)
    {
        var totalCount = engagements.Count();
        var items = engagements.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        
        return new PagedListResponse<EngagementModel>
        {
            Items = items,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}