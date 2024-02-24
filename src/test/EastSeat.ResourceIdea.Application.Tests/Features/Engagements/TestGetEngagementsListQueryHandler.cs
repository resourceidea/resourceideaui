using System.Linq.Expressions;

using AutoMapper;

using Bogus;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Engagements.DTO;
using EastSeat.ResourceIdea.Application.Features.Engagements.Handlers;
using EastSeat.ResourceIdea.Application.Features.Engagements.Queries;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Tests.Setup;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using Optional;

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
            Items = Option.Some<IReadOnlyList<Domain.Entities.Engagement>>(engagements.ToList()),
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
                       }, CancellationToken.None);

        // Assert
        Assert.IsType<List<EngagementListDTO>>(result.Items.ValueOr([]));
        Assert.Equal(3, result.Items.ValueOr([]).Count);
    }



    [Fact]
    [Trait("Feature", "Engagement")]
    public async Task CallsGetPagedListAsync_WithCorrectFilter_WhenFilterIsNotNull()
    {
        // Arrange
        var faker = new Faker();
        var fakeEngagementName = faker.Lorem.Random.Word();
        var engagements = Enumerable.Range(1, 3)
                                    .Select(e => TestEntityBuilder.Create(() => new Domain.Entities.Engagement())
                                                                  .With(engagement => engagement.Description = faker.Lorem.Sentence())
                                                                  .With(engagement => engagement.Name = fakeEngagementName));

        var engagementRepository = new Mock<IAsyncRepository<Domain.Entities.Engagement>>();
        engagementRepository.Setup(repo => repo.GetPagedListAsync(1, 10, It.IsAny<Expression<Func<Domain.Entities.Engagement, bool>>>()))
            .ReturnsAsync((int page, int size, Expression<Func<Domain.Entities.Engagement, bool>> filter) => {
                var compiledFilter = filter.Compile();
                var filteredEngagements = engagements.Where(compiledFilter).ToList();

                return new PagedList<Domain.Entities.Engagement>
                {
                    Items = Option.Some<IReadOnlyList<Domain.Entities.Engagement>>(filteredEngagements),
                    CurrentPage = 1,
                    PageSize = 10,
                    TotalCount = filteredEngagements.Count
                };
            });

        var handler = new GetEngagementsListQueryHandler(mapper, engagementRepository.Object);

        var request = new GetEngagementsListQuery
        {
            Page = 1,
            Size = 10,
            SearchKeyword = Option.Some(fakeEngagementName),
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        engagementRepository.Verify(repo => repo.GetPagedListAsync(1, 10, It.IsAny<Expression<Func<Domain.Entities.Engagement, bool>>>()), Times.Once());
        Assert.Equal(3, result.Items.ValueOr([]).Count);
    }
}
