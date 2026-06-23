using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetUserDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surename { get; set; }
        public required string Email { get; set; }
        
    }
}
