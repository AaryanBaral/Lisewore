using EmployeeManagementService.Data;
using EmployeeManagementService.Models;
using EmployeeManagementService.Interface;
using EmployeeManagementService.Service.Jwt;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public UserRepository(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // Hash password using SHA256 (for demo only — consider using stronger hashing e.g. BCrypt)
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<string> RegisterUser(Users user, string password)
        {
            user.PasswordHash = HashPassword(password);

            var parameters = new[]
            {
                new SqlParameter("@Id", user.Id),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddUser @Id, @UserName, @Email, @PasswordHash",
                parameters);

            return _jwtService.GenerateJwtToken(user);
        }

        public async Task<string> LoginUser(string email, string password)
        {
            var param = new SqlParameter("@Email", email);
            var users = await _context.Users
                .FromSqlRaw("EXEC GetUserByEmail @Email", param)
                .ToListAsync();

            var user = users.FirstOrDefault() ?? throw new Exception("Email Not Found");

            var passwordHash = HashPassword(password);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Password Not Correct");

            return _jwtService.GenerateJwtToken(user);
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
            var param = new SqlParameter("@Email", email);
            var users = await _context.Users
                .FromSqlRaw("EXEC GetUserByEmail @Email", param)
                .ToListAsync();

            return users.FirstOrDefault();
        }

        public async Task<Users?> GetUserById(string id)
        {
            var param = new SqlParameter("@Id", id);
            var users = await _context.Users
                .FromSqlRaw("EXEC GetUserById @Id", param)
                .ToListAsync();

            return users.FirstOrDefault();
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _context.Users
                .FromSqlRaw("EXEC GetAllUsers")
                .ToListAsync();
        }

        public async Task UpdateUser(Users user)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", user.Id),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash)
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateUser @Id, @UserName, @Email, @PasswordHash",
                parameters);
        }

        public async Task DeleteUserById(string id)
        {
            var param = new SqlParameter("@Id", id);
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC DeleteUser @Id",
                param);
        }
    }
}
