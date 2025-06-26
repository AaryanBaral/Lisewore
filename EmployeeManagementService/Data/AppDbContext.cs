
using EmployeeManagementService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Users>(options)
    {
        public DbSet<Employee> Employees { get; set; }
    }
}