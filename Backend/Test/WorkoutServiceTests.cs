using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;
namespace Test;
public class WorkoutServiceTests
{
    private readonly Mock<IWorkoutRepository> _repoMock;
    private readonly WorkoutService _service;

    public WorkoutServiceTests()
    {
        _repoMock = new Mock<IWorkoutRepository>();
        var validator = new WorkoutValidator();

        _service = new WorkoutService(_repoMock.Object,validator);
    }

    [Fact]
    public async Task GetMonthlyProgress_ShouldReturnSuccess_WhenWorkoutsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var year = 2026;
        var month = 2;

        _repoMock
            .Setup(r => r.GetByUserIdAndMonthAsync(userId, year, month))
            .ReturnsAsync(new List<Workout>
            {
                new Workout
                {
                    Name = "Some name",
                    WorkoutDate = new DateTime(2026, 6, 1),
                    DurationMinutes = 30,
                    Difficulty = 5,
                    Fatigue = 6
                }
            });

        // Act
        var result = await _service.GetMonthlyProgress(userId, year, month);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Weeks.Should().NotBeEmpty();
    }
    [Fact]
    public async Task GetMonthlyProgress_ShouldReturnEmptyWeeks_WhenNoWorkoutsExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var year = 2026;
        var month = 6;

        _repoMock
            .Setup(r => r.GetByUserIdAndMonthAsync(userId, year, month))
            .ReturnsAsync(new List<Workout>()); 

        // Act
        var result = await _service.GetMonthlyProgress(userId, year, month);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Year.Should().Be(year);
        result.Data.Month.Should().Be(month);
        result.Data.Weeks.Should().NotBeEmpty();
        result.Data.Weeks.Should().OnlyContain(week => week.WorkoutCount == 0);
    }
}