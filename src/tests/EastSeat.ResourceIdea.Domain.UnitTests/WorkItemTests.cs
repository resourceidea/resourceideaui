using EastSeat.ResourceIdea.Domain.Employees.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using EastSeat.ResourceIdea.Domain.WorkItems.Entities;
using EastSeat.ResourceIdea.Domain.WorkItems.Models;
using EastSeat.ResourceIdea.Domain.WorkItems.ValueObjects;

namespace EastSeat.ResourceIdea.Domain.UnitTests.WorkItems
{
    public class WorkItemTests
    {
        [Fact]
        public void ToModel_Returns_WorkItemModel_With_Correct_Values()
        {
            // Arrange
            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                Description = "Test work item description",
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
        public void ToModel_With_Employee_Names_Returns_Correct_Names()
        {
            // Arrange
            var assignedToEmployee = new EastSeat.ResourceIdea.Domain.Employees.Entities.Employee
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
                AssignedTo = assignedToEmployee
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal("John Doe", model.AssignedToName);
        }

        [Fact]
        public void ToModel_With_Null_Employee_Returns_Empty_Name()
        {
            // Arrange
            var workItem = new WorkItem
            {
                Id = WorkItemId.Create(Guid.NewGuid()),
                Title = "Test Work Item",
                TenantId = TenantId.Create(Guid.NewGuid()),
                Status = WorkItemStatus.NotStarted,
                AssignedTo = null
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(string.Empty, model.AssignedToName);
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
            var response = workItem.ToResourceIdeaResponse<WorkItem, WorkItemModel>();            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content.Value);
            Assert.Equal(workItem.Id, response.Content.Value.Id);
            Assert.Equal(workItem.Title, response.Content.Value.Title);
        }

        [Fact]
        public void ToModel_Throws_InvalidOperationException_For_Unsupported_Type()
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
            Assert.Throws<InvalidOperationException>(() => workItem.ToModel<string>());
        }

        [Fact]
        public void GetEmployeeName_Helper_Returns_Correct_Full_Name()
        {
            // This tests the helper method indirectly through ToModel
            // Arrange
            var employee = new EastSeat.ResourceIdea.Domain.Employees.Entities.Employee
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
                AssignedTo = employee
            };

            // Act
            var model = workItem.ToModel<WorkItemModel>();

            // Assert
            Assert.Equal("John Doe", model.AssignedToName);
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

        [Fact]
        public void GetEmployeeName_Helper_Handles_Empty_Names()
        {
            // Arrange
            var employee = new EastSeat.ResourceIdea.Domain.Employees.Entities.Employee
            {
                FirstName = "",
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
            Assert.Equal(string.Empty, model.AssignedToName);
        }
    }
}
