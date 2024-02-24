using System.Linq.Expressions;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Extensions;
using EastSeat.ResourceIdea.Application.Features.Engagements.DTO;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagements.Specifications;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

using Optional;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public class GetEngagementsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Engagement> engagementRepository) : IRequestHandler<GetEngagementsListQuery, PagedList<EngagementListDTO>>
{
    public async Task<PagedList<EngagementListDTO>> Handle(GetEngagementsListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Engagement, bool>> nameContainsKeywordSpecification = GetNameContainsKeywordSpecification(request);
        Expression<Func<Engagement, bool>> descriptionContainsKeywordSpecification = GetDescriptionContainsKeywordSpecification(request);
        Expression<Func<Engagement, bool>> startsAfterSpecification = GetStartsAfterSpecification(request);
        Expression<Func<Engagement, bool>> endsBeforeSpecification = GetEndsBeforeSpecification(request);
        Expression<Func<Engagement, bool>> statusSpecification = GetStatusSpecification(request);
        Expression<Func<Engagement, bool>> clientIdSpecification = GetClientIdSpecification(request);

        Expression<Func<Engagement, bool>> combinedSpecifications = nameContainsKeywordSpecification
            .Or(descriptionContainsKeywordSpecification)
            .Or(statusSpecification)
            .Or(clientIdSpecification)
            .Or(startsAfterSpecification)
            .Or(endsBeforeSpecification);

        // Use the combined specification to filter engagements
        var pagedList = await engagementRepository.GetPagedListAsync(request.Page, request.Size, combinedSpecifications);
        if (pagedList == null)
        {
            return new PagedList<EngagementListDTO>
            {
                Items = Option.None<IReadOnlyList<EngagementListDTO>>(),
                TotalCount = 0,
                PageSize = request.Size,
                CurrentPage = request.Page
            };
        }

        return new PagedList<EngagementListDTO>
        {
            Items = Option.Some(mapper.Map<IReadOnlyList<EngagementListDTO>>(pagedList.Items.ValueOr([]))),
            TotalCount = pagedList.TotalCount,
            PageSize = pagedList.PageSize,
            CurrentPage = pagedList.CurrentPage
        };
    }

    private static Expression<Func<Engagement, bool>> GetStatusSpecification(GetEngagementsListQuery request)
    {
        return request.Status.Match(
            status => EngagementSpecifications.HasStatus(status),
            () => engagement => true);
    }

    private static Expression<Func<Engagement, bool>> GetClientIdSpecification(GetEngagementsListQuery request)
    {
        return request.ClientId.Match(
            clientId => EngagementSpecifications.HasClientId(clientId),
            () => engagement => true);
    }

    private static Expression<Func<Engagement, bool>> GetEndsBeforeSpecification(GetEngagementsListQuery request)
    {
        return request.EndDate.Match(
            endDate => EngagementSpecifications.EndsBefore(endDate),
            () => engagement => true);
    }

    private static Expression<Func<Engagement, bool>> GetStartsAfterSpecification(GetEngagementsListQuery request)
    {
        return request.StartDate.Match(
            startDate => EngagementSpecifications.StartsAfter(startDate),
            () => engagement => true);
    }

    private static Expression<Func<Engagement, bool>> GetDescriptionContainsKeywordSpecification(GetEngagementsListQuery request)
    {
        return request.SearchKeyword.Match(
            keyword => EngagementSpecifications.HasDescriptionContaining(keyword),
            () => engagement => true);
    }

    private static Expression<Func<Engagement, bool>> GetNameContainsKeywordSpecification(GetEngagementsListQuery request)
    {
        // Create the specification.
        return request.SearchKeyword.Match(
            keyword => EngagementSpecifications.HasNameContaining(keyword),
            () => engagement => true);  // If no keyword is provided, match all engagements
    }

    private static Expression<Func<Engagement, bool>>? GetQueryFilter(GetEngagementsListQuery request)
    {
        Expression<Func<Engagement, bool>>? filter = null;
        if (HasQueryFilter(request))
        {
            filter = (Engagement engagement) => FilterBySearchKeyword(request, engagement) &&
                                                FilterByClientId(request, engagement) &&
                                                FilterByStartDate(request, engagement) &&
                                                FilterByEndDate(request, engagement);
        }

        return filter;
    }

    private static bool HasQueryFilter(GetEngagementsListQuery request) => request.SearchKeyword.HasValue ||
                                                                           request.ClientId.HasValue ||
                                                                           request.StartDate.HasValue ||
                                                                           request.EndDate.HasValue;

    private static bool FilterBySearchKeyword(GetEngagementsListQuery request, Engagement engagement)
    {
        return request.SearchKeyword.Match(
                        engagement.Name.Contains,
                        () => true);
    }

    private static bool FilterByClientId(GetEngagementsListQuery request, Engagement engagement)
    {
        return request.ClientId.Match(
                        clientId => engagement.ClientId == clientId,
                        () => true);
    }

    private static bool FilterByStartDate(GetEngagementsListQuery request, Engagement engagement)
    {
        return request.StartDate.Match(
                        startDate => engagement.StartDate >= startDate,
                        () => true);
    }

    private static bool FilterByEndDate(GetEngagementsListQuery request, Engagement engagement)
    {
        return request.EndDate.Match(
                        endDate => engagement.EndDate <= endDate,
                        () => true);
    }
}
