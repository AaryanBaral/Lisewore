using EmployeeManagementService.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService.Extensions
{
    public static class DataExtension
    {
            public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var postgresConnString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine(postgresConnString);
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(postgresConnString, sqloptions =>
                    sqloptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(60),
                        errorNumbersToAdd: null
                    )
            )
            .EnableDetailedErrors()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging(); // Enable sensitive data logging here
        });     
        


        return services;
    }
    }
}