using Bogus;
using EastSeat.ResourceIdea.Application.Contracts.Persistence;
using EastSeat.ResourceIdea.Application.Features.Client.Commands;
using EastSeat.ResourceIdea.Application.Features.Client.Handlers;
using EastSeat.ResourceIdea.Application.Responses;
using MediatR;

namespace EastSeat.ResourceIdea.Application.Tests.Features.Client;

public partial class DeleteClientCommandHandlerTests
{
    [Fact]
    public async void Handle_WhenValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var mockRepository = new Mock<IAsyncRepository<Domain.Entities.Client>>();
        var commandFaker = new Faker<DeleteClientCommand>()
            .RuleFor(c => c.Id, f => f.Random.Guid());
        var command = commandFaker.Generate();
        mockRepository.Setup(m => m.DeleteAsync(command.Id)).Returns(Task.FromResult(Unit.Value));
        var handler = new DeleteClientCommandHandler(mockRepository.Object);

        // When
        var result = await handler.Handle(command, CancellationToken.None);

        // Then
        Assert.IsType<BaseResponse<Unit>>(result);
        Assert.True(result.Success);
    }

    [Fact(Skip = "NotImplemented")]
    public void Handle_WhenValidRequest_IsDeleted()
    {
        // Given
    
        // When
    
        // Then
    }

    [Fact(Skip = "NotImplemented")]
    public void Handle_WhenNoClient_ReturnsFalseWithNotFoundError()
    {
        // Given
    
        // When
    
        // Then
    }
}