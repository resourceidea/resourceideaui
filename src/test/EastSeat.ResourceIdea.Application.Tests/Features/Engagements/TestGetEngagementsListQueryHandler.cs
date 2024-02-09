using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;
using EastSeat.ResourceIdea.Application.Features.Engagement.DTO;
using EastSeat.ResourceIdea.Application.Features.Engagement.Handlers;
using EastSeat.ResourceIdea.Application.Features.Engagement.Queries;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Tests.Setup;
using EastSeat.ResourceIdea.Domain.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Engagement;

public class TestGetEngagementsListQueryHandler
{
    private readonly IMapper mapper;

    public TestGetEngagementsListQueryHandler()
    {
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        mapper = mapperConfiguration.CreateMapper();
    }

    // Add test to verify that the handler returns a paged list of engagements.
    [Fact]
    [Trait("Feature", "Engagement")]
    public async Task ReturnsPagedListOfEngagements()
    {
        var engagements = Enumerable.Range(1, 3)
                                    .Select(e => TestEntityBuilder.Create(() => new Domain.Entities.Engagement())
                                                                  .With(engagement => engagement.Description = string.Empty)
                                                                  .With(engagement => engagement.Name = string.Empty));

        var engagementRepository = new Mock<IAsyncRepository<Domain.Entities.Engagement>>();
        engagementRepository.Setup(repo => repo.GetPagedListAsync(1, 10, null)).ReturnsAsync(() => new PagedList<Domain.Entities.Engagement>
        {
            Items = engagements.ToList(),
            CurrentPage = 1,
            PageSize = 10,
            TotalCount = 3
        });

        var handler = new GetEngagementsListQueryHandler(mapper, engagementRepository.Object);

        // Act
        var result = await handler.Handle(
                       new GetEngagementsListQuery
                       {
                           Page = 1,
                           Size = 10,
                           Filter = string.Empty
                       }, CancellationToken.None);

        // Assert
        Assert.IsType<List<EngagementListDTO>>(result.Items?.ToList());
        Assert.Equal(3, result.Items?.Count);
    }

    // Add test to verify that the handler returns a list of engagements.
    [Fact]
    [Trait("Feature", "Engagement")]
    public async Task ReturnsPagedListOfFilteredEngagements()
    {

    }
}
