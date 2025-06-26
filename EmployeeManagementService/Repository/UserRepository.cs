
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Interface;
using EmployeeManagementService.Models;
using EmployeeManagementService.Service.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService.Repository
{
    public class UserRepository(
        UserManager<Users> userManager,
        IJwtService jwtService
        ) : IUserRepository
    {
        private readonly UserManager<Users> _userManager = userManager;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<string> RegisterUser(Users user, string password)
        {

            var isUserCreated = await _userManager.CreateAsync(user, password) ?? throw new Exception("Error while processing identity");
            if (!isUserCreated.Succeeded)
            {
                throw new Exception(isUserCreated.Errors.FirstOrDefault()?.Description);
            }
            var token = _jwtService.GenerateJwtToken(user);
            return token;
        }

        public async Task<string> LoginUser(LoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email) ?? throw new AuthenticationFailureException("Email Not Found");
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (!isPasswordCorrect) throw new AuthenticationFailureException("Password Not Correct");
            var token = _jwtService.GenerateJwtToken(user);
            return token;
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<Users?> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task DeleteUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id) ?? throw new KeyNotFoundException("User Not Found");
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) throw new ApplicationException($"Internal Server Error: {result.Errors}");
        }

        public async Task UpdateUser(Users updatedUser)
        {
            await _userManager.UpdateAsync(updatedUser);
        }

    }
}