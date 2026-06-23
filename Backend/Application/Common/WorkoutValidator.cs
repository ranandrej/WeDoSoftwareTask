using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class WorkoutValidator
    {
        public string? Validate(int difficulty, int fatigue, int durationMinutes)
        {
            if (difficulty < 1 || difficulty > 10)
                return "Difficulty must be between 1 and 10";

            if (fatigue < 1 || fatigue > 10)
                return "Fatigue must be between 1 and 10";

            if (durationMinutes <= 0)
                return "Duration must be greater than 0";

            return null;
        }
    }
}
