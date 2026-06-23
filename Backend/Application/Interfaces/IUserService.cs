using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<GetUserDTO>> GetCurrentUser(Guid id);
    }
}

