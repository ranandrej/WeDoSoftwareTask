using Application.DTOs;
using System.Net.Mail;

namespace Application.Common
{
    public class AuthValidator
    {
        public string? ValidateRegister(RegisterDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return "Name is required";

            if (string.IsNullOrWhiteSpace(dto.Surename))
                return "Surename is required";

            var emailError = ValidateEmail(dto.Email);
            if (emailError != null)
                return emailError;

            var passwordError = ValidatePassword(dto.Password);
            if (passwordError != null)
                return passwordError;

            if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
                return "Confirm password is required";

            if (dto.Password != dto.ConfirmPassword)
                return "Passwords do not match";

            return null;
        }

        public string? ValidateLogin(LoginDTO dto)
        {
            var emailError = ValidateEmail(dto.Email);
            if (emailError != null)
                return emailError;

            var passwordError = ValidatePassword(dto.Password);
            if (passwordError != null)
                return passwordError;

            return null;
        }

        public string? ValidateUpdate(UpdateUserDTO dto)
        {
            if (dto.Name != null && string.IsNullOrWhiteSpace(dto.Name))
                return "Name cannot be empty";

            if (dto.Surename != null && string.IsNullOrWhiteSpace(dto.Surename))
                return "Surename cannot be empty";

            if (dto.Email != null)
            {
                var emailError = ValidateEmail(dto.Email);
                if (emailError != null)
                    return emailError;
            }

            var hasPassword = !string.IsNullOrWhiteSpace(dto.Password);
            var hasConfirmPassword = !string.IsNullOrWhiteSpace(dto.ConfirmPassword);

            if (hasPassword || hasConfirmPassword)
            {
                if (dto.Password != dto.ConfirmPassword)
                    return "Passwords do not match";

                var passwordError = ValidatePassword(dto.Password!);
                if (passwordError != null)
                    return passwordError;
            }

            return null;
        }

        private static string? ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email is required";

            try
            {
                _ = new MailAddress(email);
                return null;
            }
            catch (FormatException)
            {
                return "Please provide a valid email address";
            }
        }

        private static string? ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "Password is required";

            if (password.Length < 6)
                return "Password must be at least 6 characters";

            return null;
        }
    }
}
