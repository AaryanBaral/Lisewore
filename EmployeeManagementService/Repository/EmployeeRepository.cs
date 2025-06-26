using EmployeeManagementService.Data;
using EmployeeManagementService.Models;
using EmployeeManagementService.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .FromSqlRaw("EXEC GetAllEmployees")
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(string id)
        {
            var param = new SqlParameter("@Id", id);
            var employees = await _context.Employees
                .FromSqlRaw("EXEC GetEmployeeById @Id", param)
                .ToListAsync();

            return employees.FirstOrDefault();
        }

        public async Task<string> AddAsync(Employee employee)
        {
            var parameters = new[]
            {
                new SqlParameter("@EmployeeId", employee.EmployeeId),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Department", employee.Department),
                new SqlParameter("@HireDate", employee.HireDate),
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC AddEmployee @EmployeeId, @FirstName, @LastName, @Email, @Department, @HireDate", 
                parameters);

            return employee.EmployeeId;
        }

        public async Task UpdateAsync(Employee employee)
        {
            var parameters = new[]
            {
                new SqlParameter("@EmployeeId", employee.EmployeeId),
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Email", employee.Email),
                new SqlParameter("@Department", employee.Department),
                new SqlParameter("@HireDate", employee.HireDate),
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateEmployee @EmployeeId, @FirstName, @LastName, @Email, @Department, @HireDate", 
                parameters);
        }

        public async Task DeleteAsync(string id)
        {
            var param = new SqlParameter("@EmployeeId", id);
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC DeleteEmployee @EmployeeId", param);
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            var param = new SqlParameter("@Email", email);
            var employees = await _context.Employees
                .FromSqlRaw("EXEC GetEmployeeByEmail @Email", param)
                .ToListAsync();

            return employees.FirstOrDefault();
        }
    }
}
