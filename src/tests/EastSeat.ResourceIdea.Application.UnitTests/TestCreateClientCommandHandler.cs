using AutoMapper;
using EastSeat.ResourceIdea.Application.Enums;
using EastSeat.ResourceIdea.Application.Features.Clients.Commands;
using EastSeat.ResourceIdea.Application.Features.Clients.Handlers;
using EastSeat.ResourceIdea.Application.Features.Clients.Validators;
using EastSeat.ResourceIdea.Application.Features.Common.Contracts;
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
    private readonly Mock<IAsyncRepository<Client>> _mockClientRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateClientCommandHandler _handler;
    private readonly ClientId _clientId;
    private readonly TenantId _tenantId;

    public TestCreateClientCommandHandler()
    {
        _mockClientRepository = new Mock<IAsyncRepository<Client>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateClientCommandHandler(_mockClientRepository.Object, _mockMapper.Object);
        _clientId = ClientId.Create(Guid.NewGuid());
        _tenantId = TenantId.Create(Guid.NewGuid());
    }

    [Fact]
    public async Task ShouldReturnSuccess_WhenClientIsCreatedSuccessfully()
    {
        // Arrange
        var command = GetCreateClientCommand();
        var client = GetCreatedClient(command);
        var clientModel = GetExpectedResponseClientModel(client);

        var successResponse = ResourceIdeaResponse<Client>.Success(client);
        successResponse.Content = Optional<Client>.Some(client);
        _mockClientRepository.Setup(repo => repo.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResponse);

        _mockMapper.Setup(mapper => mapper.Map<ClientModel>(It.IsAny<Client>()))
            .Returns(clientModel);

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

        _mockClientRepository.Setup(repo => repo.AddAsync(It.IsAny<Client>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResourceIdeaResponse<Client>.Failure(ErrorCode.GetRepositoryFailure));

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

    private Client GetCreatedClient(CreateClientCommand command)
    {
        var client = new Client
        {
            Id = _clientId,
            Name = command.Name,
            Address = command.Address,
            TenantId = command.TenantId.Value
        };

        return client;
    }

    private ClientModel GetExpectedResponseClientModel(Client client) => new()
    {
        Id = _clientId,
        Name = client.Name,
        Address = client.Address,
        TenantId = _tenantId
    };
}