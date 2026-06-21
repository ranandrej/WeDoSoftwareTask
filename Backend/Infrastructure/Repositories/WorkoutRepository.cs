using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Migrations;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly TrainingAppDBContext _context;

        public WorkoutRepository(TrainingAppDBContext context)
        {
            _context = context;
        }

        public async Task<Workout?> GetByID(Guid id)
        {
            return await _context.Workouts.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<Workout>?> GetByUserID(Guid id)
        {
            return await _context.Workouts
                .AsNoTracking()
                .Where(w => w.UserId == id)
                .ToListAsync();
        }

        public async Task AddAsync(Workout workout)
        {
            await _context.Workouts.AddAsync(workout);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
