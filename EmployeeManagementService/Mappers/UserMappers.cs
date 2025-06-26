
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Models;

namespace EmployeeManagementService.Mappers
{
    public static class UserMappers
    {
        public static Users ToUser(RegisterUserDto registerUserDto)
        {
            return new Users()
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email
            };
        }

        public static ReadUserDto ToReadUser(Users user)
        {
            return new ReadUserDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
        public static void UpdateUser(this Users user, RegisterUserDto registerUserDto)
        {
            user.Email = registerUserDto.Email;
            user.UserName = registerUserDto.UserName;
        }
    }

}