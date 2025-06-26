using EmployeeManagementService.Data;
using EmployeeManagementService.Models;
using EmployeeManagementService.Interface;
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

        public async Task<List<Employee>> GetAllAsync() => await _context.Employees.ToListAsync();

        public async Task<Employee?> GetByIdAsync(string id) => await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

        public async Task<string> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee.EmployeeId;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var emp = await GetByIdAsync(id);
            if (emp is not null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
