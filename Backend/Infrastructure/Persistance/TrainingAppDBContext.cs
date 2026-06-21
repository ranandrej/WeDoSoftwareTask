using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class TrainingAppDBContext : DbContext

    {
        public TrainingAppDBContext(DbContextOptions<TrainingAppDBContext> options) : base(options)
        {
        }

        protected TrainingAppDBContext()
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }
    }
}
