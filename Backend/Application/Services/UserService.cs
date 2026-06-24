using Application.Common;
using Application.DTOs;
using Application.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthValidator _validator;

        public UserService(IUserRepository userRepository, AuthValidator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<Result<GetUserDTO>> GetCurrentUser(Guid id)
        {
            var currentUser = await _userRepository.GetByIdAsync(id);
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

        public async Task<Result<string>> UpdateUser(Guid id, UpdateUserDTO dto)
        {
            var validationError = _validator.ValidateUpdate(dto);
            if (validationError != null)
                return Result<string>.Fail(validationError);

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result<string>.Fail("User doesn't exist");

            if (dto.Name != null)
                user.Name = dto.Name;

            if (dto.Surename != null)
                user.Surename = dto.Surename;

            if (dto.Email != null)
            {
                if (user.Email != dto.Email)
                {
                    var emailTaken = await _userRepository.GetByEmailAsync(dto.Email);
                    if (emailTaken != null)
                        return Result<string>.Fail("User with same e-mail already exists");
                }

                user.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Result<string>.Ok("User updated successfuly");
        }

        public async Task<Result<string>> DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result<string>.Fail("User doesn't exist");

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

            return Result<string>.Ok("User deleted successfuly");
        }
    }
}
