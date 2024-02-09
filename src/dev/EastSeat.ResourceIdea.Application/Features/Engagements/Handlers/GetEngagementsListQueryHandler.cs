using System.Linq.Expressions;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Engagements.DTO;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;

public class GetEngagementsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Engagement> engagementRepository) : IRequestHandler<GetEngagementsListQuery, PagedList<EngagementListDTO>>
{
    public async Task<PagedList<EngagementListDTO>> Handle(GetEngagementsListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Domain.Entities.Engagement, bool>>? filter = null;
        filter = GetQueryFilter(request, filter);

        var pagedList = await engagementRepository.GetPagedListAsync(request.Page, request.Size, filter);
        if (pagedList == null)
        {
            return new PagedList<EngagementListDTO>
            {
                Items = Enumerable.Empty<EngagementListDTO>().ToList(),
                TotalCount = 0,
                PageSize = request.Size,
                CurrentPage = request.Page
            };
        }

        return new PagedList<EngagementListDTO>
        {
            Items = mapper.Map<IReadOnlyList<EngagementListDTO>>(pagedList.Items),
            TotalCount = pagedList.TotalCount,
            PageSize = pagedList.PageSize,
            CurrentPage = pagedList.CurrentPage
        };
    }

    private static Expression<Func<Domain.Entities.Engagement, bool>>? GetQueryFilter(GetEngagementsListQuery request, Expression<Func<Domain.Entities.Engagement, bool>>? filter)
    {
        if (HasQueryFilter(request))
        {
            filter = (Domain.Entities.Engagement engagement) =>
                FilterBySearchKeyword(request, engagement) &&
                FilterByClientId(request, engagement) &&
                FilterByStartDate(request, engagement) &&
                FilterByEndDate(request, engagement);
        }

        return filter;
    }

    private static bool HasQueryFilter(GetEngagementsListQuery request) =>
        request.SearchKeyword.HasValue ||
        request.ClientId.HasValue ||
        request.StartDate.HasValue ||
        request.EndDate.HasValue;

    private static bool FilterBySearchKeyword(GetEngagementsListQuery request, Domain.Entities.Engagement engagement)
    {
        return request.SearchKeyword.Match(
                        filter => engagement.Name.Contains(filter),
                        () => true
                    );
    }

    private static bool FilterByClientId(GetEngagementsListQuery request, Domain.Entities.Engagement engagement)
    {
        return request.ClientId.Match(
                        clientId => engagement.ClientId == clientId,
                        () => true
                    );
    }

    private static bool FilterByStartDate(GetEngagementsListQuery request, Domain.Entities.Engagement engagement)
    {
        return request.StartDate.Match(
                        startDate => engagement.StartDate >= startDate,
                        () => true
                    );
    }

    private static bool FilterByEndDate(GetEngagementsListQuery request, Domain.Entities.Engagement engagement)
    {
        return request.EndDate.Match(
                        endDate => engagement.EndDate <= endDate,
                        () => true
                    );
    }
}
