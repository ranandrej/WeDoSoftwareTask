using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Workout>> GetByUserID(Guid id)
        {
            return await _context.Workouts
                .AsNoTracking()
                .Where(w => w.UserId == id)
                .OrderByDescending(w => w.WorkoutDate)
                .ToListAsync();
        }

        public async Task<List<Workout>> GetByUserIdAndMonthAsync(Guid userId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            return await _context.Workouts
                .AsNoTracking()
                .Where(w => w.UserId == userId && w.WorkoutDate >= startDate && w.WorkoutDate < endDate)
                .ToListAsync();
        }

        public async Task AddAsync(Workout workout)
        {
            await _context.Workouts.AddAsync(workout);
        }

        public void Update(Workout workout)
        {
            _context.Workouts.Update(workout);
        }

        public void Delete(Workout workout)
        {
            _context.Workouts.Remove(workout);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
