using System.Linq.Expressions;

using AutoMapper;

using Bogus;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Clients.DTO;
using EastSeat.ResourceIdea.Application.Features.Clients.Handlers;
using EastSeat.ResourceIdea.Application.Features.Clients.Queries;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Tests.Setup;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using Optional;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public class TestGetClientsListQueryHandler
{
    private readonly IMapper mapper;

    public TestGetClientsListQueryHandler()
    {
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        mapper = mapperConfiguration.CreateMapper();
    }

    // Add test to verify that the handler returns a list of clients.
    [Fact]
    [Trait("Feature", "Client")]
    public async Task ReturnsPaginatedListOfClients()
    {
        // Arrange
        var clients = Enumerable.Range(1, 1)
                                    .Select(e => TestEntityBuilder.Create(() => new Domain.Entities.Client())
                                                                  .With(engagement => engagement.Name = "Client 1"));
        var fakePagedList = new PagedList<Domain.Entities.Client>
        {
            Items = Option.Some<IReadOnlyList<Domain.Entities.Client>>(clients.ToList())
        };
        var clientRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        clientRepository.Setup(repo => repo.GetPagedListAsync(1, 10, null)).ReturnsAsync(() => fakePagedList);

        var handler = new GetClientsListQueryHandler(mapper, clientRepository.Object);

        // Act
        var result = await handler.Handle(
            new GetClientsListQuery
            {
                Page = 1,
                Size = 10,
                Filter = string.Empty
            }, CancellationToken.None);

        // Assert
        Assert.IsType<List<ClientListDTO>>(result.Items.ValueOr([]));
        Assert.Single(result.Items.ValueOr([]));
        Assert.Equal("Client 1", result.Items.ValueOr([])[0].Name);
    }

    // Add test to verify that the handler returns a list of clients.
    [Fact]
    [Trait("Feature", "Client")]
    public async Task ReturnsPaginatedListOfFilteredClients()
    {
        // Arrange
        var faker = new Faker();
        var clients = Enumerable.Range(1, 1)
                                    .Select(e => TestEntityBuilder.Create(() => new Domain.Entities.Client())
                                                                  .With(engagement => engagement.Name = "Client 1"));
        var fakePagedList = new PagedList<Domain.Entities.Client>
        {
            Items = Option.Some<IReadOnlyList<Domain.Entities.Client>>(clients.ToList())
        };
        var clientRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        clientRepository.Setup(repo => repo.GetPagedListAsync(1, 10, It.IsAny<Expression<Func<Domain.Entities.Client, bool>>>())).ReturnsAsync(() => fakePagedList);

        var handler = new GetClientsListQueryHandler(mapper, clientRepository.Object);

        // Act
        var result = await handler.Handle(
            new GetClientsListQuery
            {
                Page = 1,
                Size = 10,
                Filter = "Client"
            }, CancellationToken.None);

        // Assert
        Assert.IsType<List<ClientListDTO>>(result.Items.ValueOr([]).ToList());
        Assert.Single(result.Items.ValueOr([]));
        Assert.Equal("Client 1", result.Items.ValueOr([])[0].Name);
    }
}
