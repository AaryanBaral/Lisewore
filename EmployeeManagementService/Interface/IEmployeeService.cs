using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementService.Dtos;

namespace EmployeeManagementService.Interface
{
    public interface IEmployeeService
    {
        Task<List<ReadEmployeeDto>> GetAllAsync();
        Task<ReadEmployeeDto?> GetByIdAsync(string id);
        Task<string> AddAsync(CreateEmployeeDto dto);
        Task<bool> UpdateAsync(string id, UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(string id);
    }
}