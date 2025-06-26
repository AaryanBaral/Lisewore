using EmployeeManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
