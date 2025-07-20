using EastSeat.ResourceIdea.Domain.Roles.Models;
using EastSeat.ResourceIdea.Domain.Tenants.ValueObjects;
using Xunit;

namespace EastSeat.ResourceIdea.Domain.UnitTests.Roles
{
    public class RoleModelTests
    {
        [Fact]
        public void IsSystemRole_Returns_True_When_BackendRole_And_EmptyTenant()
        {
            // Arrange
            var roleModel = new RoleModel
            {
                Id = "test-id",
                Name = "TestSystemRole",
                IsBackendRole = true,
                TenantId = null
            };

            // Act & Assert
            Assert.True(roleModel.IsSystemRole);
        }

        [Fact]
        public void IsSystemRole_Returns_False_When_BackendRole_But_HasTenant()
        {
            // Arrange
            var roleModel = new RoleModel
            {
                Id = "test-id",
                Name = "TestTenantRole",
                IsBackendRole = true,
                TenantId = TenantId.Create(Guid.NewGuid())
            };

            // Act & Assert
            Assert.False(roleModel.IsSystemRole);
        }

        [Fact]
        public void IsSystemRole_Returns_False_When_NotBackendRole()
        {
            // Arrange
            var roleModel = new RoleModel
            {
                Id = "test-id",
                Name = "TestRegularRole",
                IsBackendRole = false,
                TenantId = null
            };

            // Act & Assert
            Assert.False(roleModel.IsSystemRole);
        }

        [Fact]
        public void RoleModel_With_Claims_Contains_Expected_Claims()
        {
            // Arrange
            var claims = new List<RoleClaimModel>
            {
                new() { Id = 1, RoleId = "test-id", ClaimType = "permission", ClaimValue = "users.read" },
                new() { Id = 2, RoleId = "test-id", ClaimType = "permission", ClaimValue = "users.write" }
            };

            var roleModel = new RoleModel
            {
                Id = "test-id",
                Name = "TestRole",
                IsBackendRole = true,
                TenantId = null,
                Claims = claims
            };

            // Act & Assert
            Assert.Equal(2, roleModel.Claims.Count);
            Assert.Contains(roleModel.Claims, c => c.ClaimValue == "users.read");
            Assert.Contains(roleModel.Claims, c => c.ClaimValue == "users.write");
        }
    }

    public class RoleClaimModelTests
    {
        [Fact]
        public void RoleClaimModel_Properties_Set_Correctly()
        {
            // Arrange
            var roleClaim = new RoleClaimModel
            {
                Id = 123,
                RoleId = "test-role-id",
                ClaimType = "permission",
                ClaimValue = "system.admin",
                Description = "System administrator access"
            };

            // Act & Assert
            Assert.Equal(123, roleClaim.Id);
            Assert.Equal("test-role-id", roleClaim.RoleId);
            Assert.Equal("permission", roleClaim.ClaimType);
            Assert.Equal("system.admin", roleClaim.ClaimValue);
            Assert.Equal("System administrator access", roleClaim.Description);
        }
    }
}