
namespace EmployeeManagementService.Dtos
{
    public class RegisterUserDto
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class LoginUserDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class ReadUserDto
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }
}