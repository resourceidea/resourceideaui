using AutoMapper;

using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Handlers;
using EastSeat.ResourceIdea.Application.Types;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.Models;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;

using FluentAssertions;

using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests;

public class TestCreateClientCommandHandler
{
    private readonly Mock<IClientsService> _mockClientsService;
    private readonly CreateClientCommandHandler _handler;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ClientId _clientId;
    private readonly TenantId _tenantId;

    public TestCreateClientCommandHandler()
    {
        _mockClientsService = new Mock<IClientsService>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateClientCommandHandler(_mockClientsService.Object, _mockMapper.Object);
        _clientId = ClientId.Create(Guid.NewGuid());
        _tenantId = TenantId.Create(Guid.NewGuid());
    }

    [Fact]
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
        result.IsSuccess.Should().BeTrue();
        result.Content.Value.Should().BeEquivalentTo(clientModel);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = GetCreateClientCommand(isInvalidCommand: true);        

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.CreateClientCommandValidationFailure);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var command = GetCreateClientCommand();

        _mockClientsService.Setup(repo => repo.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Client>.Failure(ErrorCode.DataStoreCommandFailure));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ErrorCode.GetRepositoryFailure);
    }

    private CreateClientCommand GetCreateClientCommand(bool isInvalidCommand = false)
    {
        var command = new CreateClientCommand
        {
            Name = isInvalidCommand ? string.Empty : "Test Client",
            Address = isInvalidCommand ? Address.Empty : Address.Create("Building", "Street name", "City"),
            TenantId = _tenantId
        };

        return command;
    }

    private ClientModel GetCreatedClient(CreateClientCommand command)
    {
        var client = new ClientModel
        {
            Id = _clientId,
            Name = command.Name,
            Address = command.Address,
            TenantId = command.TenantId
        };

        return client;
    }
}