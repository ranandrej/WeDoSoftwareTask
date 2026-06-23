using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {  _userRepository = userRepository; }
        public async Task<Result<GetUserDTO>>GetCurrentUser(Guid id)
        {
           var currentUser=await _userRepository.GetByIdAsync(id);
           if (currentUser == null)
            {
                return Result<GetUserDTO>.Fail("User doesn't exist");
            }
            return Result<GetUserDTO>.Ok(new GetUserDTO
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Surename = currentUser.Surename,
                Email = currentUser.Email,
            });
        }
    }
}
