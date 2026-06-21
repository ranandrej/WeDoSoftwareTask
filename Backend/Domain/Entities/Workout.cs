using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Workout
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User? User { get; set; }
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
