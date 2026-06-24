using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwt;
        private readonly AuthValidator _validator;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenService jwt,
            AuthValidator validator)
        {
            _userRepository = userRepository;
            _jwt = jwt;
            _validator = validator;
        }

        public async Task<Result<string>> Register(RegisterDTO dto)
        {
            var validationError = _validator.ValidateRegister(dto);
            if (validationError != null)
                return Result<string>.Fail(validationError);

            var userExists = await _userRepository.GetByEmailAsync(dto.Email);
            if (userExists != null)
                return Result<string>.Fail("User with same e-mail already exists");

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
            var validationError = _validator.ValidateLogin(dto);
            if (validationError != null)
                return Result<string>.Fail(validationError);

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
