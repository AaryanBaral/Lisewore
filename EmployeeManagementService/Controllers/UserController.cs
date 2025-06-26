
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    IUserService repository
) : ControllerBase
{
    private readonly IUserService _repository = repository;


    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _repository.GetAllUsersAsync();
        return StatusCode(StatusCodes.Status200OK, new ResponseDto<List<ReadUserDto>>()
        {
            Data = users,
        });
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> CreateUsers([FromBody] RegisterUserDto registerUserDto)
    {
        var users = await _repository.RegisterAsync(registerUserDto);
        return Ok(new ResponseDto<string>()
        {
            Data = users
        });
    }

    [HttpGet]
    [Route("individual")]
    public async Task<IActionResult> GetUserById()
    {
        var userId = User.FindFirst("Id")?.Value ?? throw new UnauthorizedAccessException("Please signup");
        var result = await _repository.GetUserByIdAsync(userId);
        return Ok(new ResponseDto<ReadUserDto>()
        {
            Data = result
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody]RegisterUserDto registerUserDto)
    {
        var userId = User.FindFirst("Id")?.Value ?? throw new UnauthorizedAccessException("Please signup");
        await _repository.UpdateUserAsync(registerUserDto, userId);
        return Ok(new ResponseDto<string>()
        {
            Data = $"User successfully updated"
        });
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = User.FindFirst("Id")?.Value ?? throw new UnauthorizedAccessException("Please signup");
        await _repository.DeleteUserByIdAsync(userId);
        return Ok(new ResponseDto<string>()
        {
            Data = $"User successfully deleted"
        });
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto user)
    {
        var token = await _repository.LoginAsync(user);
        return Ok(new ResponseDto<string>()
        {
            Data = token
        });
    }
}