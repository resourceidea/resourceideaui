using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Contracts;
using EastSeat.ResourceIdea.Application.Features.Clients.Handlers;
using EastSeat.ResourceIdea.Domain.Clients.Entities;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;

using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests;

public class TestCreateClientCommandHandler
{
    private readonly Mock<IClientsService> _mockClientsService;
    private readonly AddClientCommandHandler _handler;
    private readonly ClientId _clientId;
    private readonly TenantId _tenantId;

    public TestCreateClientCommandHandler()
    {
        _mockClientsService = new Mock<IClientsService>();
        _handler = new AddClientCommandHandler(_mockClientsService.Object);
        _clientId = ClientId.Create(Guid.NewGuid());
        _tenantId = TenantId.Create(Guid.NewGuid());
    }

    [Fact]
    public async Task ShouldReturnSuccess_WhenClientIsCreatedSuccessfully()
    {
        // Arrange
        var command = new AddClientCommand
        {
            Name = "Test Client",
            City = "Test City",
            Street = "Test Street",
            Building = "Test Building",
            TenantId = _tenantId
        };

        var clientEntity = command.ToEntity();
        var expectedClient = new Client
        {
            Id = _clientId,
            Name = command.Name,
            Address = Domain.Clients.ValueObjects.Address.Create(command.Building, command.Street, command.City),
            TenantId = command.TenantId
        };

        _mockClientsService
            .Setup(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Client>.Success(Optional<Client>.Some(expectedClient)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal(command.Name, result.Content.Value.Name);
        _mockClientsService.Verify(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var command = new AddClientCommand
        {
            Name = "", // Invalid - empty name
            City = "Test City",
            Street = "Test Street",
            Building = "Test Building",
            TenantId = _tenantId
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.CommandValidationFailure, result.Error);
        _mockClientsService.Verify(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var command = new AddClientCommand
        {
            Name = "Test Client",
            City = "Test City",
            Street = "Test Street",
            Building = "Test Building",
            TenantId = _tenantId
        };

        _mockClientsService
            .Setup(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Client>.Failure(ErrorCode.DbInsertFailureOnAddClient));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.DbInsertFailureOnAddClient, result.Error);
        _mockClientsService.Verify(x => x.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}