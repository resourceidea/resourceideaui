using System.Linq.Expressions;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Engagement.DTO;
using EastSeat.ResourceIdea.Application.Features.Engagement.Queries;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using MediatR;

namespace EastSeat.ResourceIdea.Application.Features.Engagement.Handlers;

public class GetEngagementsListQueryHandler(IMapper mapper, IAsyncRepository<Domain.Entities.Engagement> engagementRepository) : IRequestHandler<GetEngagementsListQuery, PagedList<EngagementListDTO>>
{
    public async Task<PagedList<EngagementListDTO>> Handle(GetEngagementsListQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Domain.Entities.Engagement, bool>>? filter = null;

        filter = (Domain.Entities.Engagement engagement) =>
            request.SearchKeyword.Match(
                some: searchKeyword => engagement.Name.Contains(searchKeyword),
                none: () => true
            ) &&
            request.ClientId.Match(
                some: clientId => engagement.ClientId == clientId,
                none: () => true
            ) &&
            request.StartDate.Match(
                some: startDate => engagement.StartDate >= startDate,
                none: () => true
            ) &&
            request.EndDate.Match(
                some: endDate => engagement.EndDate <= endDate,
                none: () => true
            );

        var pagedList = await engagementRepository.GetPagedListAsync(request.Page, request.Size, filter);

        return new PagedList<EngagementListDTO>
        {
            Items = mapper.Map<IReadOnlyList<EngagementListDTO>>(pagedList.Items),
            TotalCount = pagedList.TotalCount,
            PageSize = pagedList.PageSize,
            CurrentPage = pagedList.CurrentPage
        };
    }
}
