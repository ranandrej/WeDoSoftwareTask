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
        Task<List<Workout>?> GetByUserID(Guid id);
        Task AddAsync(Workout workoute);
        Task SaveChangesAsync();
    }
}
