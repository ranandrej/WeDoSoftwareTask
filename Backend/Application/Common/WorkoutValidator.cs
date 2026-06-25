using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class WorkoutValidator
    {
        public string? Validate(string name, int difficulty, int fatigue, int durationMinutes, int caloriesBurned)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "Name is required";

            if (difficulty < 1 || difficulty > 10)
                return "Difficulty must be between 1 and 10";

            if (fatigue < 1 || fatigue > 10)
                return "Fatigue must be between 1 and 10";

            if (durationMinutes <= 0)
                return "Duration must be greater than 0";

            if (durationMinutes > 120)
                return "Duration must not exceed 120 minutes";

            if (caloriesBurned < 0)
                return "Calories burned cannot be negative";

            if (caloriesBurned > 2000)
                return "Calories burned must not exceed 2000";

            return null;
        }
    }
}
