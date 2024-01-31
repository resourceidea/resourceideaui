using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Features.Client.Queries;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public class TestGetClientByIdQueryHandler
{
    private readonly IMapper mapper;

    public TestGetClientByIdQueryHandler()
    {
        // Setup Mapping profile configuration.
        if (mapper == null)
        {
            var mappingConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new ApplicationMappingProfile());
            });

            mapper = mappingConfig.CreateMapper();
        }
    }

    /// <summary>
    /// Test handling of a valid request.
    /// </summary>
    /// <returns></returns>
    [Fact]
    [Trait("Feature", "Client")]
    public async Task ReturnsSuccessResponse_When_RequestIsValid()
    {
        // Given
        var mockRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        var commandFaker = new Faker<GetClientByIdQuery>()
            .RuleFor(c => c.Id, f => f.Random.Guid());
        var command = commandFaker.Generate();
        var handler = new GetClientByIdQueryHandler(mapper, mockRepository.Object);

        var clientFaker = new Faker<Domain.Entities.Client>()
            .RuleFor(c => c.Id, f => command.Id)
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.ColorCode, f => f.Random.Hexadecimal(6, string.Empty))
            .RuleFor(c => c.SubscriptionId, f => f.Random.Guid());
        var client = clientFaker.Generate();
        mockRepository.Setup(m => m.GetByIdAsync(command.Id)).ReturnsAsync(client);
    
        // When
        var result = await handler.Handle(command, CancellationToken.None);
    
        // Then
        mockRepository.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Content);
        Assert.IsType<ClientDTO>(result.Content);
    }

    /// <summary>
    /// Test handling of a request for a client that does not exist.
    /// </summary>
    /// <returns></returns>
    [Fact]
    [Trait("Feature", "Client")]
    public async Task ReturnsNotFoundResponse_When_ClientIsNotFound()
    {
        // Given
        var mockRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        var commandFaker = new Faker<GetClientByIdQuery>()
            .RuleFor(c => c.Id, f => f.Random.Guid());
        var command = commandFaker.Generate();
        var handler = new GetClientByIdQueryHandler(mapper, mockRepository.Object);

        mockRepository.Setup(m => m.GetByIdAsync(command.Id)).ReturnsAsync(() => null);
    
        // When
        var result = await handler.Handle(command, CancellationToken.None);
    
        // Then
        mockRepository.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        Assert.NotNull(result);
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.False(result.Success);
        Assert.Null(result.Content);
        Assert.Equal(Constants.ErrorCodes.NotFound, result.ErrorCode);
        Assert.NotNull(result.Errors);
        Assert.Single(result.Errors);
        Assert.Equal(Constants.ErrorCodes.NotFound, result.Errors.First());
    }
}
