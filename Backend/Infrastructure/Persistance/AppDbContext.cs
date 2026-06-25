using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected AppDbContext()
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Workout> Workouts { get; set; }
}
