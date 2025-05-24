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
    private readonly ClientId _clientId;

    public TestCreateClientCommandHandler()
    {
        _mockClientsService = new Mock<IClientsService>();
        _clientId = ClientId.Create(Guid.NewGuid());
    }

    [Fact(Skip = "To be implemented")]
    public Task ShouldReturnSuccess_WhenClientIsCreatedSuccessfully()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public Task ShouldReturnFailure_WhenValidationFails()
    {
        throw new NotImplementedException();
    }

    [Fact(Skip = "To be implemented")]
    public Task ShouldReturnFailure_WhenRepositoryFails()
    {
        throw new NotImplementedException();
    }
}