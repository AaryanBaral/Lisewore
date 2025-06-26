using System.Text;
using System.Text.Json;
using Cafe_Management_System.Exceptions;
using EmployeeManagementService.Configurations;
using EmployeeManagementService.Interface;
using EmployeeManagementService.Repository;
using EmployeeManagementService.Service;
using EmployeeManagementService.Service.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementService.Extensions
{
    public static class ServiceExtension
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddCorsConfiguration();
            services.AddExceptionHandler<GlobalException>();
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.AddScoped<IJwtService, JwtService>();
            services.AddJwtAuthentication(configuration);
            services.AddRepositories();
            services.AddDatabase(configuration);
        }

        private static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = configuration.GetSection("JwtConfig:Secret").Value;
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT secret is missing in configuration");
            }

            var key = Encoding.ASCII.GetBytes(secret);
            var tokenValidationParameters = new TokenValidationParameters()
            {
                //used to validate token using different options
                ValidateIssuerSigningKey = true, //to validate the tokens signing key
                IssuerSigningKey = new SymmetricSecurityKey(key), // we compare if it matches our key or not
                ValidateIssuer = false, // it issued to validate the issuer
                ValidateAudience = false, // it issued to validate the issuer
                RequireExpirationTime = false, //it sets the token is not expired 
                ValidateLifetime = true // it sets that the token is valid for lifetime
            };

            // Add the Authentication scheme and configurations
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // Add the jwt configurations as what should be done and how to do it
                .AddJwtBearer(jwt =>
                {
                    jwt.SaveToken = true; // saves the generated token to http context
                    jwt.TokenValidationParameters = tokenValidationParameters;
                    jwt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var response = new
                            {
                                success = false,
                                message = context.Exception.Message
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";

                            var response = new
                            {
                                success = false,
                                message = "You are not authorized."
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";

                            var response = new
                            {
                                success = false,
                                message = "You do not have permission to access this resource."
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        }
                    };
                });
            services.AddSingleton(tokenValidationParameters);
        }

        private static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {

                options.AddPolicy("AllowAny", builder =>
                {
                    builder.WithOrigins("http://localhost:3000", "http://localhost:3001")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }
    }
}