using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
      
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surename { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public ICollection<Workout>? workouts { get; set; }
        public User() { }


    }
}
