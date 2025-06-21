using System;
using EastSeat.ResourceIdea.Application.Features.Engagements.Commands;
using EastSeat.ResourceIdea.Application.Mappers;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Application.UnitTests.Mappers
{
    public class EngagementMapperTests
    {
        [Fact]
        public void ToEntity_FromCreateEngagementCommand_Returns_Engagement_With_Correct_Values()
        {
            // Arrange
            var command = new CreateEngagementCommand
            {
                ClientId = ClientId.Create(Guid.NewGuid()),
                Title = "Test Engagement Title",
                Description = "Test engagement description",
                DueDate = DateTimeOffset.UtcNow.AddDays(30),
                Status = EngagementStatus.NotStarted,
                TenantId = TenantId.Create(Guid.NewGuid())
            };

            // Act
            var entity = command.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.NotEqual(EngagementId.Empty, entity.Id);
            Assert.Equal(command.Title, entity.Title);
            Assert.Equal(command.Description, entity.Description); // This should be the direct mapping, not concatenated
            Assert.Equal(command.ClientId, entity.ClientId);
            Assert.Equal(command.DueDate, entity.EndDate);
            Assert.Equal(command.Status, entity.EngagementStatus);
            Assert.Equal(command.TenantId, entity.TenantId);
            Assert.Null(entity.StartDate); // StartDate should be null for new engagements
            Assert.Null(entity.ManagerId); // ManagerId should be null for new engagements
            Assert.Null(entity.PartnerId); // PartnerId should be null for new engagements
        }

        [Fact]
        public void ToEntity_FromCreateEngagementCommand_Description_Should_Not_Include_Title()
        {
            // Arrange
            var command = new CreateEngagementCommand
            {
                ClientId = ClientId.Create(Guid.NewGuid()),
                Title = "My Title",
                Description = "My Description",
                TenantId = TenantId.Create(Guid.NewGuid())
            };

            // Act
            var entity = command.ToEntity();

            // Assert
            Assert.Equal("My Description", entity.Description);
            Assert.DoesNotContain("My Title", entity.Description); // Description should not contain the title
        }

        [Fact]
        public void ToEntity_FromCreateEngagementCommand_With_Empty_Description_Returns_Empty_String()
        {
            // Arrange
            var command = new CreateEngagementCommand
            {
                ClientId = ClientId.Create(Guid.NewGuid()),
                Title = "Test Title",
                Description = "",
                TenantId = TenantId.Create(Guid.NewGuid())
            };

            // Act
            var entity = command.ToEntity();

            // Assert
            Assert.Equal(string.Empty, entity.Description);
        }

        [Fact]
        public void ToEntity_FromCreateEngagementCommand_With_Null_Description_Returns_Empty_String()
        {
            // Arrange
            var command = new CreateEngagementCommand
            {
                ClientId = ClientId.Create(Guid.NewGuid()),
                Title = "Test Title",
                Description = null,
                TenantId = TenantId.Create(Guid.NewGuid())
            };

            // Act
            var entity = command.ToEntity();

            // Assert
            Assert.Equal(string.Empty, entity.Description);
        }

        [Fact]
        public void ToEntity_FromEngagementModel_Returns_Engagement_With_Correct_Values()
        {
            // Arrange
            var model = new EngagementModel
            {
                Id = EngagementId.Create(Guid.NewGuid()),
                Title = "Test Model Title",
                Description = "Test model description",
                ClientId = ClientId.Create(Guid.NewGuid()),
                TenantId = TenantId.Create(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow.AddDays(-5),
                EndDate = DateTimeOffset.UtcNow.AddDays(25),
                Status = EngagementStatus.InProgress
            };

            // Act
            var entity = model.ToEntity();

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(model.Id, entity.Id);
            Assert.Equal(model.Title, entity.Title);
            Assert.Equal(model.Description, entity.Description);
            Assert.Equal(model.ClientId, entity.ClientId);
            Assert.Equal(model.TenantId, entity.TenantId);
            Assert.Equal(model.StartDate, entity.StartDate);
            Assert.Equal(model.EndDate, entity.EndDate);
            Assert.Equal(model.Status, entity.EngagementStatus);
            Assert.Equal(model.ManagerId, entity.ManagerId);
            Assert.Equal(model.PartnerId, entity.PartnerId);
        }
    }
}