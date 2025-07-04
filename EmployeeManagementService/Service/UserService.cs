using System.Security.Cryptography;
using System.Text;
using System.Xml;
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Interface;
using EmployeeManagementService.Mappers;


namespace EmployeeManagementService.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
            private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task<string> RegisterAsync(RegisterUserDto dto)
        {
            var user = UserMappers.ToUser(dto, HashPassword(dto.Password));
            return await _userRepository.RegisterUser(user,dto.Password);
        }

        public async Task<string> LoginAsync(LoginUserDto dto)
        {
            return await _userRepository.LoginUser(dto.Email, dto.Password);
        }

        public async Task<ReadUserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email) ?? throw new KeyNotFoundException("user of given email not found");
            return UserMappers.ToReadUser(user);
        }

        public async Task<List<ReadUserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsers();
            return users.ConvertAll(UserMappers.ToReadUser);
        }

        public async Task<ReadUserDto> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetUserById(id) ?? throw new KeyNotFoundException("user of given id not found");
            return Mappers.UserMappers.ToReadUser(user);
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            await _userRepository.DeleteUserById(id);
        }

        public async Task UpdateUserAsync(RegisterUserDto updateUserDto, string userId)
        {
            var user = await _userRepository.GetUserById(userId) ?? throw new KeyNotFoundException("user of given id not found");
            user.UpdateUser(updateUserDto);
            await _userRepository.UpdateUser(user);
        }
    }
}