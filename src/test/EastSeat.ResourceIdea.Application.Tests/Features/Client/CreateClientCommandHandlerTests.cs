using System.Text.RegularExpressions;

using AutoMapper;

using Bogus;

using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.DTO;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Profiles;
using EastSeat.ResourceIdea.Application.Responses;
using EastSeat.ResourceIdea.Domain.Common;
using EastSeat.ResourceIdea.Domain.Entities;
using EastSeat.ResourceIdea.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Update.Internal;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public partial class CreateClientCommandHandlerTests
{
    private readonly Guid subscriptionId;
    private readonly IMapper mapper;

    public CreateClientCommandHandlerTests()
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
    public async Task Handle_WhenValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var command = new Faker<CreateClientCommand>()
            .RuleFor(c => c.Name, f => new NonEmptyString(f.Company.CompanyName()))
            .RuleFor(c => c.SubscriptionId, subscriptionId)
            .RuleFor(c => c.Address, f => new NonEmptyString(f.Address.FullAddress()))
            .RuleFor(c => c.ColorCode, f => f.Random.Hexadecimal(6, string.Empty));

        var handler = new CreateClientCommandHandler(mapper, mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsType<BaseResponse<ClientDTO>>(result);
        Assert.True(result.Success);
    }

    [Fact]
    [Trait("Feature", "Client")]
    public async Task Handle_WhenColorCodeIsNotValid_ReturnsFailureResponse()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var fakeCommand = new Faker<CreateClientCommand>()
            .RuleFor(c => c.Name, f => new NonEmptyString(f.Company.CompanyName()))
            .RuleFor(c => c.SubscriptionId, subscriptionId)
            .RuleFor(c => c.Address, f => new NonEmptyString(f.Address.FullAddress()))
            .RuleFor(c => c.ColorCode, f => f.Random.String2(8));
        var command = fakeCommand.Generate();
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
        Assert.Contains("Invalid client color code.", result.Errors);
    }

    [Fact]
    [Trait("Feature", "Client")]
    public async Task Handle_WhenMissingClientName_ReturnsFailureResponse()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var fakeCommand = new Faker<CreateClientCommand>()
            .RuleFor(c => c.Name, new NonEmptyString(Constants.Strings.NonEmptyString))
            .RuleFor(c => c.SubscriptionId, subscriptionId)
            .RuleFor(c => c.Address, f => new NonEmptyString(f.Address.FullAddress()))
            .RuleFor(c => c.ColorCode, f => f.Random.Hexadecimal(6, string.Empty));
        var command = fakeCommand.Generate();
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
        Assert.Contains("Client name is required.", result.Errors);
    }

    [Fact]
    [Trait("Feature", "Client")]
    public async Task Handle_WhenEmptySubscriptionGuid_ReturnsFailureResponse()
    {
        // Arrange
        var mockRepository = new Mock<IClientRepository>();
        var fakeCommand = new Faker<CreateClientCommand>()
            .RuleFor(c => c.Name, f => new NonEmptyString(f.Company.CompanyName()))
            .RuleFor(c => c.SubscriptionId, Guid.Empty)
            .RuleFor(c => c.Address, f => new NonEmptyString(f.Address.FullAddress()))
            .RuleFor(c => c.ColorCode, f => f.Random.Hexadecimal(6, string.Empty));
        var command = fakeCommand.Generate();
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
