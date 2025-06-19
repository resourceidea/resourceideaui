using EastSeat.ResourceIdea.Application.Features.WorkItems.Commands;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Contracts;
using EastSeat.ResourceIdea.Application.Features.WorkItems.Handlers;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;

using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.WorkItems.Handlers;

/// <summary>
/// Unit tests for <see cref="CreateWorkItemCommandHandler"/>.
/// </summary>
public class CreateWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemsService> _mockWorkItemsService;
    private readonly CreateWorkItemCommandHandler _handler;

    public CreateWorkItemCommandHandlerTests()
    {
        _mockWorkItemsService = new Mock<IWorkItemsService>();
        _handler = new CreateWorkItemCommandHandler(_mockWorkItemsService.Object);
    }

    [Fact]
    public async Task Handle_WhenServiceReturnsSuccess_ShouldReturnSuccessResponse()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            Description = "Test description",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1),
            Priority = Priority.High
        };

        var workItem = command.ToEntity();
        var serviceResponse = ResourceIdeaResponse<WorkItem>.Success(workItem);

        _mockWorkItemsService
            .Setup(s => s.AddAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal(command.Title, result.Content.Value.Title);
        Assert.Equal(command.Description, result.Content.Value.Description);
        Assert.Equal(command.EngagementId, result.Content.Value.EngagementId);
        Assert.Equal(command.Priority, result.Content.Value.Priority);
    }

    [Fact]
    public async Task Handle_WhenServiceReturnsFailure_ShouldReturnFailureResponse()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var serviceResponse = ResourceIdeaResponse<WorkItem>.Failure(ErrorCode.DataStoreCommandFailure);

        _mockWorkItemsService
            .Setup(s => s.AddAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.DataStoreCommandFailure, result.Error);
    }

    [Fact]
    public async Task Handle_WhenServiceReturnsSuccessButContentIsEmpty_ShouldReturnEmptyEntityError()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid())
        };

        var serviceResponse = ResourceIdeaResponse<WorkItem>.Success(default(WorkItem));

        _mockWorkItemsService
            .Setup(s => s.AddAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.EmptyEntityOnCreateWorkItem, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldCallWorkItemsServiceWithCorrectEntity()
    {
        // Arrange
        var command = new CreateWorkItemCommand
        {
            Title = "Test Work Item",
            Description = "Test description",
            EngagementId = EngagementId.Create(Guid.NewGuid()),
            TenantId = TenantId.Create(Guid.NewGuid()),
            StartDate = DateTimeOffset.UtcNow.AddDays(1),
            Priority = Priority.Low
        };

        var workItem = command.ToEntity();
        var serviceResponse = ResourceIdeaResponse<WorkItem>.Success(workItem);

        _mockWorkItemsService
            .Setup(s => s.AddAsync(It.IsAny<WorkItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResponse);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockWorkItemsService.Verify(s => s.AddAsync(
            It.Is<WorkItem>(w =>
                w.Title == command.Title &&
                w.Description == command.Description &&
                w.EngagementId == command.EngagementId &&
                w.TenantId == command.TenantId &&
                w.StartDate == command.StartDate &&
                w.Priority == command.Priority &&
                w.Status == WorkItemStatus.NotStarted),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}