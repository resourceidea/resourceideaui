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
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Domain.ValueObjects;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public class GetClientsListQueryHandlerTests
{
    private readonly IMapper mapper;

    public GetClientsListQueryHandlerTests()
    {
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        mapper = mapperConfiguration.CreateMapper();
    }

    // Add test to verify that the handler returns a list of clients.
    [Fact]
    [Trait("Feature", "Client")]
    public async Task Handler_ReturnsPaginatedListOfClients()
    {
        // Arrange
        var fakePagedList = new PagedList<Domain.Entities.Client>
        {
            Items = [
            new Domain.Entities.Client { Id = Guid.NewGuid(), Name = "Client 1", Address = "Address 1", ColorCode = "#000000", SubscriptionId = Guid.NewGuid() },
            new Domain.Entities.Client { Id = Guid.NewGuid(), Name = "Client 2", Address = "Address 2", ColorCode = "#000000", SubscriptionId = Guid.NewGuid() },
            new Domain.Entities.Client { Id = Guid.NewGuid(), Name = "Client 3", Address = "Address 3", ColorCode = "#000000", SubscriptionId = Guid.NewGuid() }
        ]
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
        Assert.IsType<List<ClientListDTO>>(result.Items?.ToList());
        Assert.Equal(3, result.Items?.Count);
        Assert.Equal("Client 1", result.Items?[0].Name);
        Assert.Equal("Client 2", result.Items?[1].Name);
        Assert.Equal("Client 3", result.Items?[2].Name);
    }

    // Add test to verify that the handler returns a list of clients.
    [Fact]
    [Trait("Feature", "Client")]
    public async Task Handler_ReturnsPaginatedListOfFilteredClients()
    {
        // Arrange
        var fakePagedList = new PagedList<Domain.Entities.Client>
        {
            Items = [
            new Domain.Entities.Client { Id = Guid.NewGuid(), Name = "Client 1", Address = "Address 1", ColorCode = "#000000", SubscriptionId = Guid.NewGuid() },
            new Domain.Entities.Client { Id = Guid.NewGuid(), Name = "Client 2", Address = "Address 2", ColorCode = "#000000", SubscriptionId = Guid.NewGuid() },
            new Domain.Entities.Client { Id = Guid.NewGuid(), Name = "Client 3", Address = "Address 3", ColorCode = "#000000", SubscriptionId = Guid.NewGuid() }
        ]
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
        Assert.IsType<List<ClientListDTO>>(result.Items?.ToList());
        Assert.Equal(3, result.Items?.Count);
        Assert.Equal("Client 1", result.Items?[0].Name);
        Assert.Equal("Client 2", result.Items?[1].Name);
        Assert.Equal("Client 3", result.Items?[2].Name);
    }
}
