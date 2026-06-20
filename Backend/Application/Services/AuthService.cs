using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwt;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenService jwt)
        {
            _userRepository = userRepository;
            _jwt = jwt;
        }

        public async Task<string> Register(RegisterDTO dto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surename=dto.Surename,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _jwt.CreateToken(user);
        }

        public async Task<string?> Login(LoginDTO dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return null;

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!valid)
                return null;

            return _jwt.CreateToken(user);
        }
    }
}
