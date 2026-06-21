using Application.Common;
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

        public async Task<Result<string>> Register(RegisterDTO dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return Result<string>.Fail("Passwords do not match");

            var userExists = await _userRepository.GetByEmailAsync(dto.Email);
            if (userExists != null)
                return Result<string>.Fail("User already exists");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surename = dto.Surename,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _jwt.CreateToken(user);

            return Result<string>.Ok(token);
        }

        public async Task<Result<string>> Login(LoginDTO dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return Result<string>.Fail("User doesn't exist");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!valid)
                return Result<string>.Fail("Invalid credentials");

            var token = _jwt.CreateToken(user);

            return Result<string>.Ok(token);
        }
    }
}
