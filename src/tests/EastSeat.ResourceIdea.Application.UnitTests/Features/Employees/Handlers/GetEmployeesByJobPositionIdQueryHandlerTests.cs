// ============================================================================================
// File: GetEmployeesByJobPositionIdQueryHandlerTests.cs
// Path: src\tests\EastSeat.ResourceIdea.Application.UnitTests\Features\Employees\Handlers\GetEmployeesByJobPositionIdQueryHandlerTests.cs
// Description: Unit tests for GetEmployeesByJobPositionIdQueryHandler
// ============================================================================================

using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.JobPositions.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Enums;
using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

public class GetEmployeesByJobPositionIdQueryHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly GetEmployeesByJobPositionIdQueryHandler _handler;

    public GetEmployeesByJobPositionIdQueryHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new GetEmployeesByJobPositionIdQueryHandler(_mockEmployeeService.Object);
    }

    [Fact]
    public async Task Handle_WithValidQuery_ShouldReturnPagedEmployees()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var jobPositionId = JobPositionId.Create(Guid.NewGuid());
        var query = new GetEmployeesByJobPositionIdQuery
        {
            TenantId = tenantId,
            JobPositionId = jobPositionId,
            PageNumber = 1,
            PageSize = 10
        };

        var pagedResponse = new PagedListResponse<Employee>
        {
            Items = new List<Employee>(),
            TotalCount = 0,
            CurrentPage = 1,
            PageSize = 10
        };

        var response = ResourceIdeaResponse<PagedListResponse<Employee>>.Success(pagedResponse);

        _mockEmployeeService
            .Setup(s => s.GetPagedListAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);

        _mockEmployeeService.Verify(
            s => s.GetPagedListAsync(
                1,
                10,
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenServiceFails_ShouldReturnFailure()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var jobPositionId = JobPositionId.Create(Guid.NewGuid());
        var query = new GetEmployeesByJobPositionIdQuery
        {
            TenantId = tenantId,
            JobPositionId = jobPositionId,
            PageNumber = 1,
            PageSize = 10
        };

        var response = ResourceIdeaResponse<PagedListResponse<Employee>>.Failure(ErrorCode.DataStoreQueryFailure);

        _mockEmployeeService
            .Setup(s => s.GetPagedListAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
    }
}