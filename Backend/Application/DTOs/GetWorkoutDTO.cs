using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetWorkoutDTO
    {
        public Guid Id { get; set; }
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
