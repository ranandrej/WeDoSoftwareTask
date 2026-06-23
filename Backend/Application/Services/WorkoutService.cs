using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;

namespace Application.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly WorkoutValidator _validator;
        public WorkoutService(IWorkoutRepository workoutRepository,WorkoutValidator validator)
        {
            _workoutRepository = workoutRepository;
            _validator = validator;
        }

        public async Task<Result<string>> CreateWorkout(CreateWorkoutDTO dto, Guid userId)
        {
            var validationError = _validator.Validate(dto.Difficulty, dto.Fatigue, dto.DurationMinutes);
            if (validationError != null)
                return Result<string>.Fail(validationError);

            var workout = new Workout
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                DurationMinutes = dto.DurationMinutes,
                Fatigue = dto.Fatigue,
                Difficulty = dto.Difficulty,
                CaloriesBurned = dto.CaloriesBurned,
                Notes = dto.Notes,
                WorkoutDate = dto.WorkoutDate,
                UserId = userId
            };

            await _workoutRepository.AddAsync(workout);
            await _workoutRepository.SaveChangesAsync();
            return Result<string>.Ok("Workout created successfuly");
        }

        public async Task<Result<List<GetWorkoutDTO>>> GetWorkoutsForUser(Guid userId)
        {
            var workouts = await _workoutRepository.GetByUserID(userId);
            var dtos = workouts.Select(workout => new GetWorkoutDTO
            {
                Id = workout.Id,
                Name = workout.Name,
                Type = workout.Type,
                DurationMinutes = workout.DurationMinutes,
                CaloriesBurned = workout.CaloriesBurned,
                Difficulty = workout.Difficulty,
                Fatigue = workout.Fatigue,
                Notes = workout.Notes,
                WorkoutDate = workout.WorkoutDate
            }).ToList();
            return Result<List<GetWorkoutDTO>>.Ok(dtos);
        }

        public async Task<Result<GetWorkoutDTO>> GetWorkoutById(Guid workoutId, Guid userId)
        {
            var workout = await _workoutRepository.GetByID(workoutId);

            if (workout == null || workout.UserId != userId)
                return Result<GetWorkoutDTO>.Fail("Workout not found");


            return Result<GetWorkoutDTO>.Ok(new GetWorkoutDTO
                  {
                      Id = workout.Id,
                      Name = workout.Name,
                      Type = workout.Type,
                      DurationMinutes = workout.DurationMinutes,
                      CaloriesBurned = workout.CaloriesBurned,
                      Difficulty = workout.Difficulty,
                      Fatigue = workout.Fatigue,
                      Notes = workout.Notes,
                      WorkoutDate = workout.WorkoutDate
                  }
            );
            }

        public async Task<Result<string>> UpdateWorkout(Guid workoutId, UpdateWorkoutDTO dto, Guid userId)
        {
            var workout = await _workoutRepository.GetByID(workoutId);

            if (workout == null || workout.UserId != userId)
                return Result<string>.Fail("Workout not found");

            var validationError = _validator.Validate(dto.Difficulty, dto.Fatigue, dto.DurationMinutes);
            if (validationError != null)
                return Result<string>.Fail(validationError);

            workout.Name = dto.Name;
            workout.Type = dto.Type;
            workout.DurationMinutes = dto.DurationMinutes;
            workout.CaloriesBurned = dto.CaloriesBurned;
            workout.Difficulty = dto.Difficulty;
            workout.Fatigue = dto.Fatigue;
            workout.Notes = dto.Notes;
            workout.WorkoutDate = dto.WorkoutDate;

            _workoutRepository.Update(workout);
            await _workoutRepository.SaveChangesAsync();
            return Result<string>.Ok("Workout updated successfuly");
        }

        public async Task<Result<string>> DeleteWorkout(Guid workoutId, Guid userId)
        {
            var workout = await _workoutRepository.GetByID(workoutId);

            if (workout == null || workout.UserId != userId)
                return Result<string>.Fail("Workout not found");

            _workoutRepository.Delete(workout);
            await _workoutRepository.SaveChangesAsync();
            return Result<string>.Ok("Workout deleted successfuly");
        }

        public async Task<Result<MonthlyProgressDTO>> GetMonthlyProgress(Guid userId, int year, int month)
        {
            if (month < 1 || month > 12)
                return Result<MonthlyProgressDTO>.Fail("Month must be between 1 and 12");

            if (year < 1)
                return Result<MonthlyProgressDTO>.Fail("Invalid year");

            var workouts = await _workoutRepository.GetByUserIdAndMonthAsync(userId, year, month);
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var weekCount = (daysInMonth + 6) / 7;

            var weeks = new List<WeeklyProgressDTO>();

            for (var weekNumber = 1; weekNumber <= weekCount; weekNumber++)
            {
                var weekWorkouts = workouts
                    .Where(w => DateUtils.GetWeekOfMonth(w.WorkoutDate) == weekNumber)
                    .ToList();

                weeks.Add(new WeeklyProgressDTO
                {
                    WeekNumber = weekNumber,
                    TotalDurationMinutes = weekWorkouts.Sum(w => w.DurationMinutes),
                    WorkoutCount = weekWorkouts.Count,
                    AverageDifficulty = weekWorkouts.Count == 0
                        ? 0
                        : Math.Round(weekWorkouts.Average(w => w.Difficulty), 2),
                    AverageFatigue = weekWorkouts.Count == 0
                        ? 0
                        : Math.Round(weekWorkouts.Average(w => w.Fatigue), 2)
                });
            }

            return Result<MonthlyProgressDTO>.Ok(new MonthlyProgressDTO
            {
                Year = year,
                Month = month,
                Weeks = weeks
            });
        }

        public Result<List<string>> GetWorkoutTypes()
        {
            var types = Enum.GetNames(typeof(WorkoutType)).ToList();
            return Result<List<string>>.Ok(types);
        }


    }
}
