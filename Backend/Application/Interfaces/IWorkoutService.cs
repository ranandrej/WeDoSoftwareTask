using Application.Common;
using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<Result<string>> CreateWorkout(CreateWorkoutDTO dto, Guid userId);
        Task<Result<List<GetWorkoutDTO>>> GetWorkoutsForUser(Guid userId, int year, int month, bool descending);
        Task<Result<GetWorkoutDTO>> GetWorkoutById(Guid workoutId, Guid userId);
        Task<Result<string>> UpdateWorkout(Guid workoutId, UpdateWorkoutDTO dto, Guid userId);
        Task<Result<string>> DeleteWorkout(Guid workoutId, Guid userId);
        Task<Result<MonthlyProgressDTO>> GetMonthlyProgress(Guid userId, int year, int month);
        Result<List<string>> GetWorkoutTypes();
    }
}
