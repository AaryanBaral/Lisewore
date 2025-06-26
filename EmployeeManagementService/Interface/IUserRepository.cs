using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Models;

namespace EmployeeManagementService.Interface
{
    public interface IUserRepository
    {
        Task<string> RegisterUser(Users user, string password);
        Task<string> LoginUser(LoginUserDto loginUserDto);
        Task<Users?> GetUserByEmail(string email);
        Task<List<Users>> GetAllUsers();
        Task<Users?> GetUserById(string id);
        Task DeleteUserById(string id);
        Task UpdateUser(Users updatedUser);
    }
}