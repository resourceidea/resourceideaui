using System;
using Xunit;
using EastSeat.ResourceIdea.Web;
using EastSeat.ResourceIdea.Domain.Exceptions;
using System.Reflection;

namespace EastSeat.ResourceIdea.Web.UnitTests;

public class AppStartupConfigurationTests
{
    [Fact]
    public void GetUserEnvironmentVariable_ShouldReturnProcessEnvironmentVariable_WhenAvailable()
    {
        // Arrange
        string testKey = "TEST_PROCESS_ENV_VAR";
        string testValue = "process_test_value";
        Environment.SetEnvironmentVariable(testKey, testValue);

        try
        {
            // Act
            string result = InvokeGetUserEnvironmentVariable(testKey);

            // Assert
            Assert.Equal(testValue, result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testKey, null);
        }
    }

    [Fact]
    public void GetUserEnvironmentVariable_ShouldReturnUserEnvironmentVariable_WhenProcessVariableNotAvailable()
    {
        // Arrange
        string testKey = "TEST_USER_ENV_VAR_UNIQUE";
        string testValue = "user_test_value";
        
        // Ensure process variable is not set
        Environment.SetEnvironmentVariable(testKey, null);
        
        // Set user environment variable - Note: This test might not work in all environments
        // but demonstrates the intended behavior
        try 
        {
            Environment.SetEnvironmentVariable(testKey, testValue, EnvironmentVariableTarget.User);
            
            // Check if we can read it back (this validates the test environment supports it)
            var readBack = Environment.GetEnvironmentVariable(testKey, EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(readBack))
            {
                // Act
                string result = InvokeGetUserEnvironmentVariable(testKey);

                // Assert
                Assert.Equal(testValue, result);
            }
            else
            {
                // Skip this test if user environment variables aren't supported
                Assert.True(true, "User environment variables not supported in this test environment");
            }
        }
        finally
        {
            // Cleanup
            try
            {
                Environment.SetEnvironmentVariable(testKey, null, EnvironmentVariableTarget.User);
            }
            catch
            {
                // Ignore cleanup errors
            }
        }
    }

    [Fact]
    public void GetUserEnvironmentVariable_ShouldPreferProcessOverUser_WhenBothAvailable()
    {
        // Arrange
        string testKey = "TEST_BOTH_ENV_VAR";
        string processValue = "process_value";
        string userValue = "user_value";
        
        Environment.SetEnvironmentVariable(testKey, processValue);
        Environment.SetEnvironmentVariable(testKey, userValue, EnvironmentVariableTarget.User);

        try
        {
            // Act
            string result = InvokeGetUserEnvironmentVariable(testKey);

            // Assert
            Assert.Equal(processValue, result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testKey, null);
            Environment.SetEnvironmentVariable(testKey, null, EnvironmentVariableTarget.User);
        }
    }

    [Fact]
    public void GetUserEnvironmentVariable_ShouldThrowResourceIdeaException_WhenVariableNotFound()
    {
        // Arrange
        string testKey = "NONEXISTENT_ENV_VAR_12345";
        
        // Ensure variable is not set in either location
        Environment.SetEnvironmentVariable(testKey, null);
        Environment.SetEnvironmentVariable(testKey, null, EnvironmentVariableTarget.User);

        // Act & Assert
        var exception = Assert.Throws<TargetInvocationException>(() => InvokeGetUserEnvironmentVariable(testKey));
        Assert.NotNull(exception.InnerException);
        Assert.IsType<ResourceIdeaException>(exception.InnerException);
        Assert.Contains("Failed to retrieve environment variable", exception.InnerException.Message);
        Assert.Contains(testKey, exception.InnerException.Message);
    }

    [Fact]
    public void GetUserEnvironmentVariable_ShouldThrowResourceIdeaException_WhenVariableIsEmpty()
    {
        // Arrange
        string testKey = "TEST_EMPTY_ENV_VAR";
        Environment.SetEnvironmentVariable(testKey, "");

        try
        {
            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() => InvokeGetUserEnvironmentVariable(testKey));
            Assert.NotNull(exception.InnerException);
            Assert.IsType<ResourceIdeaException>(exception.InnerException);
            Assert.Contains("Failed to retrieve environment variable", exception.InnerException.Message);
            Assert.Contains(testKey, exception.InnerException.Message);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testKey, null);
        }
    }

    [Fact]
    public void GetUserEnvironmentVariable_ShouldThrowResourceIdeaException_WhenVariableIsWhiteSpace()
    {
        // Arrange
        string testKey = "TEST_WHITESPACE_ENV_VAR";
        Environment.SetEnvironmentVariable(testKey, "   ");

        try
        {
            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() => InvokeGetUserEnvironmentVariable(testKey));
            Assert.NotNull(exception.InnerException);
            Assert.IsType<ResourceIdeaException>(exception.InnerException);
            Assert.Contains("Failed to retrieve environment variable", exception.InnerException.Message);
            Assert.Contains(testKey, exception.InnerException.Message);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testKey, null);
        }
    }

    /// <summary>
    /// Helper method to invoke the private GetUserEnvironmentVariable method via reflection
    /// </summary>
    private static string InvokeGetUserEnvironmentVariable(string environmentVariableKey)
    {
        var method = typeof(AppStartupConfiguration).GetMethod("GetUserEnvironmentVariable", 
            BindingFlags.NonPublic | BindingFlags.Static);
        
        if (method == null)
            throw new InvalidOperationException("GetUserEnvironmentVariable method not found");

        return (string)method.Invoke(null, new object[] { environmentVariableKey })!;
    }
}