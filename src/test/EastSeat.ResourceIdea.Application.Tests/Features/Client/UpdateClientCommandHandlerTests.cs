using AutoMapper;
using Bogus;
using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public class UpdateClientCommandHandlerTests
{
    private readonly Guid subscriptionId;
    private readonly IMapper mapper;

    public UpdateClientCommandHandlerTests()
    {        
        // Setup the id for the subscription.
        subscriptionId = new("0373ba5e-b03c-4883-a30e-f4a30fe6b53d");

        // Setup Mapping profile configuration.
        if (mapper == null)
        {
            var mappingConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile(new ApplicationMappingProfile());
            });

            mapper = mappingConfig.CreateMapper();
        }
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_WhenValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var mockRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        var commandFaker = new Faker<UpdateClientCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.SubscriptionId, subscriptionId)
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.ColorCode, f => f.Random.Hexadecimal(6, string.Empty));
        var command = commandFaker.Generate();
        var handler = new UpdateClientCommandHandler(mapper, mockRepository.Object);
        var client = new Domain.Entities.Client
        {
            Id = command.Id,
            Name = command.Name,
            Address = command.Address,
            ColorCode = command.ColorCode,
            SubscriptionId = command.SubscriptionId
        };
        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(client);
        mockRepository.Setup(m => m.UpdateAsync(It.IsAny<Domain.Entities.Client>())).ReturnsAsync(client);

        // When
        var result = await handler.Handle(command, CancellationToken.None);
    
        // Then
        mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Domain.Entities.Client>()), Times.Once);
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.True(result.Success);
        Assert.Equal(command.Id, result.Content?.Id);
        Assert.Equal(command.SubscriptionId, result.Content?.SubscriptionId);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_WhenClientNotFound_ReturnsFailureResponse()
    {
        // Given
        var mockRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        var commandFaker = new Faker<UpdateClientCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.SubscriptionId, subscriptionId)
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.ColorCode, f => f.Random.Hexadecimal(6, string.Empty));
        var command = commandFaker.Generate();
        var handler = new UpdateClientCommandHandler(mapper, mockRepository.Object);
        mockRepository.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null);

        // When
        var result = await handler.Handle(command, CancellationToken.None);

        // Then
        mockRepository.Verify(m => m.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.False(result.Success);
        Assert.Equal(Constants.ErrorCodes.NotFound, result.ErrorCode);
        Assert.Contains(Constants.ErrorCodes.NotFound, result.Errors ?? []);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task Handle_WhenValidationFails_ReturnsFailureResponse()
    {
        // Given
        var mockRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        var commandFaker = new Faker<UpdateClientCommand>()
            .RuleFor(c => c.Id, Guid.Empty)
            .RuleFor(c => c.Name, string.Empty)
            .RuleFor(c => c.SubscriptionId, Guid.Empty)
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.ColorCode, string.Empty);
        var command = commandFaker.Generate();
        var handler = new UpdateClientCommandHandler(mapper, mockRepository.Object);

        // When
        var result = await handler.Handle(command, CancellationToken.None);

        // Then
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.False(result.Success);
        Assert.Equal("ValidationFailure", result.ErrorCode);
        Assert.Contains("Client ID is required.", result.Errors ?? []);
        Assert.Contains("Client name is required.", result.Errors ?? []);
        Assert.Contains("Invalid client color code.", result.Errors ?? []);
        Assert.Contains("Subscription ID is required.", result.Errors ?? []);
    }
}