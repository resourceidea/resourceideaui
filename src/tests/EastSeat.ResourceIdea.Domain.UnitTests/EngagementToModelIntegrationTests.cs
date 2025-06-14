using System;
using EastSeat.ResourceIdea.Domain.Engagements.Entities;
using EastSeat.ResourceIdea.Domain.Engagements.Models;
using EastSeat.ResourceIdea.Domain.Engagements.ValueObjects;
using EastSeat.ResourceIdea.Domain.Enums;
using EastSeat.ResourceIdea.Domain.Clients.ValueObjects;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Domain.UnitTests.Engagements
{
    public class EngagementToModelIntegrationTests
    {
        [Fact]
        public void Engagement_ToModel_And_ToResourceIdeaResponse_Integration_Test()
        {
            // Arrange
            var engagement = new Engagement
            {
                Id = EngagementId.Create(Guid.NewGuid()),
                ClientId = ClientId.Create(Guid.NewGuid()),
                TenantId = TenantId.Create(Guid.NewGuid()),
                StartDate = DateTimeOffset.UtcNow.AddDays(-30),
                EndDate = DateTimeOffset.UtcNow.AddDays(30),
                EngagementStatus = EngagementStatus.InProgress,
                Description = "Integration test engagement"
            };

            // Act
            var model = engagement.ToModel<EngagementModel>();
            var response = engagement.ToResourceIdeaResponse<Engagement, EngagementModel>();

            // Assert
            Assert.NotNull(model);
            Assert.NotNull(response);
            Assert.True(response.IsSuccess);
            Assert.True(response.Content.HasValue);
            Assert.Equal(model.Id, response.Content.Value.Id);
            Assert.Equal(model.Description, response.Content.Value.Description);
        }
    }
}
