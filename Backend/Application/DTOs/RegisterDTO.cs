using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surename{ get; set; }

        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string ConfirmPassword { get; set; }
    }
}
