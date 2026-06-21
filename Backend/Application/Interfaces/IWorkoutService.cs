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
        Task<Result<string>> CreateWorkout(CreateWorkoutDTO dto,Guid userId);
        Task<Result<List<Workout>>> GetWorkoutsForUser(Guid userId);

    }
}
