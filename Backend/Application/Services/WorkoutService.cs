using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class WorkoutService:IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public WorkoutService(
            IWorkoutRepository workoutRepository,
            IJwtTokenService jwtTokenService
            ) 
        {
         _jwtTokenService = jwtTokenService;
         _workoutRepository = workoutRepository;
        }
        public async Task<Result<string>> CreateWorkout(CreateWorkoutDTO dto,Guid userId)
        {
            var workout = new Workout
            {
                Id = new Guid(),
                Name = dto.Name,
                Type = dto.Type,
                DurationMinutes = dto.DurationMinutes,
                Fatigue = dto.Fatigue,
                Difficulty = dto.Difficulty,
                CaloriesBurned = dto.CaloriesBurned,
                Notes = dto.Notes,
                WorkoutDate = dto.WorkoutDate
            };
            workout.UserId = userId;
            await _workoutRepository.AddAsync( workout );
            await _workoutRepository.SaveChangesAsync();
            return Result<string>.Ok("Workout created successfuly");

        }
        public async Task<Result<List<Workout>>> GetWorkoutsForUser(Guid userId)
        {
            var workouts = await _workoutRepository.GetByUserID(userId);

            if (!workouts.Any() || workouts == null)
                return Result<List<Workout>>.Fail("No workouts found");

            return Result<List<Workout>>.Ok(workouts);
        }

    }

}
