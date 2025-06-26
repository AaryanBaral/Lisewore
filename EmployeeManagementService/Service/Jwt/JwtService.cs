using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagementService.Configurations;
using EmployeeManagementService.Data;
using EmployeeManagementService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementService.Service.Jwt
{

public class JwtService(
    IOptions<JwtConfig> config,
    AppDbContext context,
    TokenValidationParameters tokenValidationParameters):IJwtService
{
    private readonly JwtConfig _config = config.Value;
    private readonly AppDbContext _context = context;
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;

    public string GenerateJwtToken(Users user)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // convert the string into byte of arrays
            var key = Encoding.UTF8.GetBytes(_config.Secret);
            /* Claims
                this is used to add key-value pair of data that should be encrypted
                and added to the jwt token
            */
            var claims = new ClaimsIdentity([
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub,
                    user.Email ?? throw new ArgumentNullException(nameof(user), "User's Email cannot be null")),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
            ]);

            /*
                A token descriptor describes the properites and values to be in the token
            */
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.Add(_config.ExpiryTimeFrame),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);


            // //  creating a new refresh token
            // var refreshToken = new RefreshToken()
            // {
            //     JwtId = token.Id,
            //     Token = RandomStringGenerator(23), // Generate a refresh token
            //     ExpiryDate = DateTime.UtcNow.AddMonths(6),
            //     UserId = user.Id,
            //     IsRevoked = false,
            //     IsUsed = false,
            //     AddedDate = DateTime.UtcNow,
            // };

            // adding the refresh token to the database
            // await _context.RefreshTokens.AddAsync(refreshToken);
            // await _context.SaveChangesAsync();

            return jwtToken;
        }
        catch (Exception ex)
        {
            throw new Exception($"Server Error {ex.Message}");
        }
    }
}
}