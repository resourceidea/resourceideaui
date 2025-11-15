using EastSeat.ResourceIdea.Domain.Services;
using Xunit;

namespace EastSeat.ResourceIdea.Domain.Tests.Services;

public class CalendarServiceTests
{
    [Theory]
    [InlineData("2025-01-04", false)] // Saturday
    [InlineData("2025-01-05", false)] // Sunday
    [InlineData("2025-01-06", true)]  // Monday
    public void IsWeekend_ShouldReturnCorrectValue(string dateStr, bool expectedIsWorkday)
    {
        // Arrange
        var date = DateTime.Parse(dateStr);

        // Act
        var isWeekend = CalendarService.IsWeekend(date);

        // Assert
        Assert.Equal(!expectedIsWorkday, isWeekend);
    }

    [Fact]
    public void CalculateWorkingDays_ShouldExcludeWeekendsAndHolidays()
    {
        // Arrange
        var startDate = new DateTime(2025, 1, 1); // Wednesday
        var endDate = new DateTime(2025, 1, 10);   // Friday
        var holidays = new[] { new DateTime(2025, 1, 1) }; // New Year's Day

        // Act
        var workingDays = CalendarService.CalculateWorkingDays(startDate, endDate, holidays);

        // Assert
        // Jan 1-10: Wed(1-holiday), Thu(2), Fri(3), Sat, Sun, Mon(6), Tue(7), Wed(8), Thu(9), Fri(10)
        // Working days: 2,3,6,7,8,9,10 = 7 days
        Assert.Equal(7, workingDays);
    }

    [Fact]
    public void GetWorkingDays_ShouldReturnOnlyWorkingDays()
    {
        // Arrange
        var startDate = new DateTime(2025, 1, 6);  // Monday
        var endDate = new DateTime(2025, 1, 10);   // Friday
        var holidays = new DateTime[] { };

        // Act
        var workingDays = CalendarService.GetWorkingDays(startDate, endDate, holidays).ToList();

        // Assert
        Assert.Equal(5, workingDays.Count);
        Assert.All(workingDays, day => Assert.False(CalendarService.IsWeekend(day)));
    }
}
