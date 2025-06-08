using System;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Types;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Domain.UnitTests.Engagements
{
    public class EngagementTests
    {
        [Fact]
        public void ToModel_Returns_EngagementModel_With_Correct_Values()
        {
            // Arrange
            var engagement = new Engagement
            {
                Id = EngagementId.Create(Guid.NewGuid()),
                ClientId = ClientId.Create(Guid.NewGuid()),
                TenantId = TenantId.Create(Guid.NewGuid()),
                CommencementDate = DateTimeOffset.UtcNow.AddDays(-10),
                CompletionDate = DateTimeOffset.UtcNow,
                EngagementStatus = EngagementStatus.Completed,
                Description = "Test engagement"
            };

            // Act
            var model = engagement.ToModel<EngagementModel>();

            // Assert
            Assert.NotNull(model);
            Assert.Equal(engagement.Id, model.Id);
            Assert.Equal(engagement.ClientId, model.ClientId);
            Assert.Equal(engagement.TenantId, model.TenantId);
            Assert.Equal(engagement.CommencementDate, model.CommencementDate);
            Assert.Equal(engagement.CompletionDate, model.CompletionDate);
            Assert.Equal(engagement.EngagementStatus, model.Status);
            Assert.Equal(engagement.Description, model.Description);
        }

        [Fact]
        public void ToResourceIdeaResponse_Returns_Success_Response_With_Mapped_Model()
        {
            // Arrange
            var engagement = new Engagement
            {
                Id = EngagementId.Create(Guid.NewGuid()),
                ClientId = ClientId.Create(Guid.NewGuid()),
                TenantId = TenantId.Create(Guid.NewGuid()),
                CommencementDate = DateTimeOffset.UtcNow.AddDays(-10),
                CompletionDate = DateTimeOffset.UtcNow,
                EngagementStatus = EngagementStatus.InProgress,
                Description = "Test engagement for response"
            };

            // Act
            var response = engagement.ToResourceIdeaResponse<Engagement, EngagementModel>();

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.True(response.Content.HasValue);
            var model = response.Content.Value;
            Assert.Equal(engagement.Id, model.Id);
            Assert.Equal(engagement.ClientId, model.ClientId);
            Assert.Equal(engagement.TenantId, model.TenantId);
            Assert.Equal(engagement.CommencementDate, model.CommencementDate);
            Assert.Equal(engagement.CompletionDate, model.CompletionDate);
            Assert.Equal(engagement.EngagementStatus, model.Status);
            Assert.Equal(engagement.Description, model.Description);
        }

        [Fact]
        public void ToModel_With_Invalid_Type_Throws_InvalidOperationException()
        {
            // Arrange
            var engagement = new Engagement();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => engagement.ToModel<string>());
        }

        [Fact]
        public void ToResourceIdeaResponse_With_Invalid_Type_Throws_InvalidOperationException()
        {
            // Arrange
            var engagement = new Engagement();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => engagement.ToResourceIdeaResponse<Engagement, string>());
        }
    }
}
