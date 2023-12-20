using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;
using EastSeat.ResourceIdea.Application.Profiles;

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
    [Trait("Category", "Unit")]
    public async Task Handler_ReturnsListOfClients()
    {
        // Arrange
        List<Domain.Entities.Client> mockClientList = [
            It.IsAny<Domain.Entities.Client>(),
            It.IsAny<Domain.Entities.Client>(),
            It.IsAny<Domain.Entities.Client>()
        ];
        var clientRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        clientRepository.Setup(repo => repo.ListAllAsync()).ReturnsAsync(() => mockClientList);

        var handler = new GetClientsListQueryHandler(mapper, clientRepository.Object);

        // Act
        var result = await handler.Handle(new GetClientsListQuery(), CancellationToken.None);

        // Assert
        Assert.IsType<List<ClientListDTO>>(result);
        Assert.Equal(3, result.Count);
    }
}
