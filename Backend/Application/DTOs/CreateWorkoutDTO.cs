using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateWorkoutDTO
    {
        public required string Name { get; set; }
        public WorkoutType Type { get; set; }

        public int DurationMinutes { get; set; }

        public int CaloriesBurned { get; set; }

        public int Difficulty { get; set; } // 1-10

        public int Fatigue { get; set; } // 1-10

        public string? Notes { get; set; }

        public DateTime WorkoutDate { get; set; }
    }
}
