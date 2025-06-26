
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Models;

namespace EmployeeManagementService.Interface
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterUserDto dto);
        Task<string> LoginAsync(LoginUserDto dto);
        Task<ReadUserDto> GetUserByEmailAsync(string email);
        Task<List<ReadUserDto>> GetAllUsersAsync();
        Task<ReadUserDto> GetUserByIdAsync(string id);
        Task DeleteUserByIdAsync(string id);
        Task UpdateUserAsync(RegisterUserDto registerUserDto, string userId); 
    }
}