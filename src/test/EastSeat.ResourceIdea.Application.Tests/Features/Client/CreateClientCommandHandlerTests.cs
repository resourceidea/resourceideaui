using System.Text.RegularExpressions;

using AutoMapper;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Responses;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public partial class TestCreateClientCommandHandler
{
    private readonly Guid subscriptionId;
    private readonly IMapper mapper;

    public TestCreateClientCommandHandler()
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
    [Trait("Feature", "Client")]
    public async Task ReturnsSuccessResponse_When_RequestIsValid()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var command = new CreateClientCommand
        {
            Name = "Company name",
            SubscriptionId = subscriptionId,
            Address = "Address 1",
            ColorCode = "00FFBB"
        };

        // Act
        var handler = new CreateClientCommandHandler(mapper, mockRepository.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.True(result.Success);
    }

    [Fact]
    [Trait("Feature", "Client")]
    public async Task ReturnsFailureResponse_When_ColorCodeIsNotValid()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var command = new CreateClientCommand
        {
            Name = "Company name",
            SubscriptionId = subscriptionId,
            Address = "Address 1",
            ColorCode = "00FFB"
        };
        var fakeClient = new Domain.Entities.Client
        {
            Id = command.Id,
            Name = command.Name,
            SubscriptionId = command.SubscriptionId,
            Address = command.Address,
            ColorCode = command.ColorCode
        };
        mockRepository.Setup(repo => repo.AddAsync(fakeClient)).Returns(Task.FromResult(fakeClient));
        var handler = new CreateClientCommandHandler(mapper, mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
    }

    [Fact]
    [Trait("Feature", "Client")]
    public void ReturnsFailureResponse_When_ClientNameIsMissing()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        Assert.Throws<ArgumentException>(() => new CreateClientCommand
        {
            Name = string.Empty,
            SubscriptionId = subscriptionId,
            Address = "Address 1",
            ColorCode = "#00FFBB"
        });
    }

    [Fact]
    [Trait("Feature", "Client")]
    public async Task ReturnsFailureResponse_When_SubscriptionIdIsEmptyGuid()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var command = new CreateClientCommand
        {
            Name = "Company name",
            SubscriptionId = Guid.Empty,
            Address = "Address 1",
            ColorCode = "#00FFBB"
        };
        var fakeClient = new Domain.Entities.Client
        {
            Id = command.Id,
            Name = command.Name,
            SubscriptionId = command.SubscriptionId,
            Address = command.Address,
            ColorCode = command.ColorCode
        };
        mockRepository.Setup(repo => repo.AddAsync(fakeClient)).Returns(Task.FromResult(fakeClient));
        var handler = new CreateClientCommandHandler(mapper, mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Contains("Empty Subscription ID is not allowed.", result.Errors);
    }

    [GeneratedRegex(@"^[a-fA-F0-9]*$")]
    private static partial Regex ColorCodeGenerator();
}
