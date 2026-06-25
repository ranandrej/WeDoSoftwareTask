using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWorkoutRepository
    {
        Task<Workout?> GetByID(Guid id);
        Task<List<Workout>> GetByUserAsync(Guid userId, int year, int month, bool descending);
        Task<List<Workout>> GetByUserIdAndMonthAsync(Guid userId, int year, int month);
        Task AddAsync(Workout workout);
        void Update(Workout workout);
        void Delete(Workout workout);
        Task SaveChangesAsync();
    }
}
