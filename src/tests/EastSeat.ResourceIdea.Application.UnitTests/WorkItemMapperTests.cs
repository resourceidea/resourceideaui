using EastSeat.ResourceIdea.Application.Features.Common.ValueObjects;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Application.UnitTests.Mappers
{
    public class WorkItemMapperTests
    {
        [Fact]
        public void ToModel_Returns_WorkItemModel_With_Correct_Values()
        {
            // Arrange
            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                Description = "Test description",
                EngagementId = EngagementId.Create(Guid.NewGuid()),
                TenantId = TenantId.Create(Guid.NewGuid()),
                PlannedStartDate = DateTimeOffset.UtcNow.AddDays(-5),
                CompletedDate = DateTimeOffset.UtcNow,
                Status = WorkItemStatus.Completed,
                Priority = Priority.Critical,
                AssignedToId = EmployeeId.Create(Guid.NewGuid())
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(workItem.Id, model.Id);
            Assert.Equal(workItem.Title, model.Title);
            Assert.Equal(workItem.Description, model.Description);
            Assert.Equal(workItem.EngagementId, model.EngagementId);
            Assert.Equal(workItem.TenantId, model.TenantId);
            Assert.Equal(workItem.PlannedStartDate, model.StartDate);
            Assert.Equal(workItem.CompletedDate, model.CompletedDate);
            Assert.Equal(workItem.Status, model.Status);
            Assert.Equal(workItem.Priority, model.Priority);
            Assert.Equal(workItem.AssignedToId, model.AssignedToId);
        }

        [Fact]
        public void ToModel_With_Engagement_Returns_EngagementTitle()
        {
            // Arrange
            var engagement = new EastSeat.ResourceIdea.Domain.Engagements.Entities.Engagement
            {
                Title = "Test Engagement"
            };

            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress,
                Engagement = engagement
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("Test Engagement", model.EngagementTitle);
        }

        [Fact]
        public void ToModel_With_Employee_Returns_EmployeeName()
        {
            // Arrange
            var assignedTo = new EastSeat.ResourceIdea.Domain.Employees.Entities.Employee
            {
                FirstName = "John",
                LastName = "Doe"
            };

            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress,
                AssignedTo = assignedTo
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("John Doe", model.AssignedToName);
        }
        [Fact]
        public void ToModel_Throws_NotSupportedException_For_Unsupported_Type()
        {
            // Arrange
            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress
            };

            // Act & Assert
            // Note: This test uses the WorkItemMapper.ToModel extension method, which throws NotSupportedException
            // The WorkItem.ToModel method (from BaseEntity) throws InvalidOperationException
            Assert.Throws<NotSupportedException>(() => WorkItemMapper.ToModel<string>(workItem));
        }

        [Fact]
        public void ToEntity_Returns_WorkItem_With_Correct_Values()
        {
            // Arrange
            var model = new WorkItemModel
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                Description = "Test description",
                EngagementId = EngagementId.Create(Guid.NewGuid()),
                TenantId = TenantId.Create(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow.AddDays(-5),
                CompletedDate = DateTimeOffset.UtcNow,
                Status = WorkItemStatus.Completed,
                Priority = Priority.High,
                AssignedToId = EmployeeId.Create(Guid.NewGuid())
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.Title, entity.Title);
            Assert.Equal(model.Description, entity.Description);
            Assert.Equal(model.EngagementId, entity.EngagementId);
            Assert.Equal(model.TenantId, entity.TenantId);
            Assert.Equal(model.StartDate, entity.PlannedStartDate);
            Assert.Equal(model.CompletedDate, entity.CompletedDate);
            Assert.Equal(model.Status, entity.Status);
            Assert.Equal(model.Priority, entity.Priority);
            Assert.Equal(model.AssignedToId, entity.AssignedToId);
        }

        [Fact]
        public void ToResourceIdeaResponse_Returns_Success_Response()
        {
            // Arrange
            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress
            };

            // Act
            var response = workItem.ToResourceIdeaResponse();            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content.Value);
            Assert.Equal(workItem.Id, response.Content.Value.Id);
            Assert.Equal(workItem.Title, response.Content.Value.Title);
        }

        [Fact]
        public void ToResourceIdeaResponse_PagedList_Returns_Success_Response()
        {
            // Arrange
            var workItems = new[]
            {
                new WorkItem
                {
                    Id = WorkItemId.Create(Guid.NewGuid()),
                    Title = "Work Item 1",
                    TenantId = TenantId.Create(Guid.NewGuid()),
                    Status = WorkItemStatus.InProgress
                },
                new WorkItem
                {
                    Id = WorkItemId.Create(Guid.NewGuid()),
                    Title = "Work Item 2",
                    TenantId = TenantId.Create(Guid.NewGuid()),
                    Status = WorkItemStatus.Completed
                }
            };

            var pagedList = new PagedListResponse<WorkItem>
            {
                Items = workItems.ToList(),
                CurrentPage = 1,
                PageSize = 10,
                TotalCount = 2
            };

            // Act
            var response = pagedList.ToResourceIdeaResponse();            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content.Value);
            Assert.Equal(2, response.Content.Value.Items.Count);
            Assert.Equal(1, response.Content.Value.CurrentPage);
            Assert.Equal(10, response.Content.Value.PageSize);
            Assert.Equal(2, response.Content.Value.TotalCount);
            Assert.Equal("Work Item 1", response.Content.Value.Items[0].Title);
            Assert.Equal("Work Item 2", response.Content.Value.Items[1].Title);
        }

        [Fact]
        public void GetEmployeeName_Helper_Handles_Null_Employee()
        {
            // Arrange
            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress,
                AssignedTo = null
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(string.Empty, model.AssignedToName);
        }

        [Fact]
        public void GetEmployeeName_Helper_Handles_Only_FirstName()
        {
            // Arrange
            var employee = new EastSeat.ResourceIdea.Domain.Employees.Entities.Employee
            {
                FirstName = "John",
                LastName = ""
            };

            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress,
                AssignedTo = employee
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.Equal("John", model.AssignedToName);
        }

        [Fact]
        public void GetEmployeeName_Helper_Handles_Only_LastName()
        {
            // Arrange
            var employee = new EastSeat.ResourceIdea.Domain.Employees.Entities.Employee
            {
                FirstName = "",
                LastName = "Doe"
            };

            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.InProgress,
                AssignedTo = employee
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.Equal("Doe", model.AssignedToName);
        }
    }
}
