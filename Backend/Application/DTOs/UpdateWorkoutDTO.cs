using Domain.Enums;

namespace Application.DTOs
{
    public class UpdateWorkoutDTO
    {
        public required string Name { get; set; }
        public WorkoutType Type { get; set; }
        public int DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public int Difficulty { get; set; }
        public int Fatigue { get; set; }
        public string? Notes { get; set; }
        public DateTime WorkoutDate { get; set; }
    }
}
