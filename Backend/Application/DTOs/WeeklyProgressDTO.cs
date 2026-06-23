namespace Application.DTOs
{
    public class WeeklyProgressDTO
    {
        public int WeekNumber { get; set; }
        public int TotalDurationMinutes { get; set; }
        public int WorkoutCount { get; set; }
        public double AverageDifficulty { get; set; }
        public double AverageFatigue { get; set; }
    }
}
