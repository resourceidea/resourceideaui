using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Handlers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;

using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests;

public class TestCreateClientCommandHandler
{
    private readonly Mock<IClientsService> _mockClientsService;
    private readonly CreateClientCommandHandler _handler;
    private readonly ClientId _clientId;

    public TestCreateClientCommandHandler()
    {
        _mockClientsService = new Mock<IClientsService>();
        _handler = new CreateClientCommandHandler(_mockClientsService.Object);
        _clientId = ClientId.Create(Guid.NewGuid());
    }

    [Fact(Skip = "To be implemented")]
    public async Task ShouldReturnSuccess_WhenClientIsCreatedSuccessfully()
    {
        // Arrange
        var command = GetCreateClientCommand();
        var clientModel = GetCreatedClient(command);

        _mockClientsService.Setup(repo => repo.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(It.IsAny<ResourceIdeaResponse<Client>>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(clientModel, result.Content.Value);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = GetCreateClientCommand(isInvalidCommand: true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.CommandValidationFailure, result.Error);
    }

    [Fact(Skip = "To be implemented")]
    public async Task ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var command = GetCreateClientCommand();

        _mockClientsService.Setup(repo => repo.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Client>.Failure(ErrorCode.DataStoreCommandFailure));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.GetRepositoryFailure, result.Error);
    }

    private CreateClientCommand GetCreateClientCommand(bool isInvalidCommand = false)
    {
        var command = new CreateClientCommand
        {
            Name = isInvalidCommand ? string.Empty : "Test Client",
            Address = isInvalidCommand ? Address.Empty : Address.Create("Building", "Street name", "City")
        };

        return command;
    }

    private ClientModel GetCreatedClient(CreateClientCommand command)
    {
        var client = new ClientModel
        {
            ClientId = _clientId,
            Name = command.Name,
            Address = command.Address
        };

        return client;
    }
}