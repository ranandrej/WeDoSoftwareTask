namespace Application.DTOs
{
    public class RegisterDTO
    {
        public required string Name { get; set; }
        public required string Surename { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
