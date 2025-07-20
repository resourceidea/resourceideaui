using EastSeat.ResourceIdea.Application.Features.Common.Specifications;
using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Features.Employees.Contracts;
using EastSeat.ResourceIdea.Application.Features.Employees.Handlers;
using EastSeat.ResourceIdea.Application.Features.Employees.Queries;
using EastSeat.ResourceIdea.Application.Features.Employees.Specifications;
using EastSeat.ResourceIdea.Domain.Employees.Entities;
using EastSeat.ResourceIdea.Domain.Employees.Models;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.Types;
using Moq;

namespace EastSeat.ResourceIdea.Application.UnitTests.Features.Employees.Handlers;

/// <summary>
/// Unit tests for <see cref="GetPotentialManagersQueryHandler"/>.
/// </summary>
public class GetPotentialManagersQueryHandlerTests
{
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly GetPotentialManagersQueryHandler _handler;

    public GetPotentialManagersQueryHandlerTests()
    {
        _mockEmployeeService = new Mock<IEmployeeService>();
        _handler = new GetPotentialManagersQueryHandler(_mockEmployeeService.Object);
    }

    [Fact]
    public async Task Handle_WhenQueryIsValid_ShouldReturnSuccessResponse()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var excludeEmployeeId = EmployeeId.Create(Guid.NewGuid());
        var query = new GetPotentialManagersQuery
        {
            TenantId = tenantId,
            ExcludeEmployeeId = excludeEmployeeId,
            PageNumber = 1,
            PageSize = 10
        };

        var employees = new List<Employee>
        {
            CreateTestEmployee("John", "Doe"),
            CreateTestEmployee("Jane", "Smith")
        };

        var pagedResponse = new PagedListResponse<Employee>
        {
            Items = employees,
            TotalCount = 2,
            CurrentPage = 1,
            PageSize = 10
        };

        var serviceResponse = ResourceIdeaResponse<PagedListResponse<Employee>>.Success(pagedResponse);

        _mockEmployeeService
            .Setup(s => s.GetPagedListAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Content.HasValue);
        Assert.Equal(2, result.Content.Value.TotalCount);
        Assert.Equal(2, result.Content.Value.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenQueryValidationFails_ShouldReturnValidationFailure()
    {
        // Arrange
        var query = new GetPotentialManagersQuery
        {
            // Missing required fields
            TenantId = TenantId.Empty,
            ExcludeEmployeeId = EmployeeId.Empty
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCode.EmployeeQueryValidationFailure, result.Error);
    }

    [Fact]
    public async Task Handle_ShouldCallServiceWithCorrectParameters()
    {
        // Arrange
        var tenantId = TenantId.Create(Guid.NewGuid());
        var excludeEmployeeId = EmployeeId.Create(Guid.NewGuid());
        var query = new GetPotentialManagersQuery
        {
            TenantId = tenantId,
            ExcludeEmployeeId = excludeEmployeeId,
            PageNumber = 2,
            PageSize = 20
        };

        var pagedResponse = new PagedListResponse<Employee>
        {
            Items = new List<Employee>(),
            TotalCount = 0,
            CurrentPage = 2,
            PageSize = 20
        };

        var serviceResponse = ResourceIdeaResponse<PagedListResponse<Employee>>.Success(pagedResponse);

        _mockEmployeeService
            .Setup(s => s.GetPagedListAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Optional<BaseSpecification<Employee>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceResponse);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockEmployeeService.Verify(s => s.GetPagedListAsync(
            2, // PageNumber
            20, // PageSize
            It.IsAny<Optional<BaseSpecification<Employee>>>(), // Use Optional wrapper
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Employee CreateTestEmployee(string firstName, string lastName)
    {
        return new Employee
        {
            EmployeeId = EmployeeId.Create(Guid.NewGuid()),
            FirstName = firstName,
            LastName = lastName,
            Email = $"{firstName.ToLower()}.{lastName.ToLower()}@test.com",
            TenantId = TenantId.Create(Guid.NewGuid())
        };
    }
}