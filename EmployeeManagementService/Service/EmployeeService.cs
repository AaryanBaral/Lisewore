using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Interface;
using EmployeeManagementService.Mappers;

namespace EmployeeManagementService.Service
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReadEmployeeDto>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return employees.Select(EmployeeMappers.ToReadDto).ToList();
        }

        public async Task<ReadEmployeeDto?> GetByIdAsync(string id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return employee is not null ? EmployeeMappers.ToReadDto(employee) : null;
        }

        public async Task<string> AddAsync(CreateEmployeeDto dto)
        {
            var employeeById = await _repository.GetEmployeeByEmailAsync(dto.Email);
            if (employeeById is not null) throw new InvalidOperationException("Email Already Exists");
            var employee = EmployeeMappers.ToEmployee(dto);
            var id = await _repository.AddAsync(employee);
            return id;
        }

        public async Task<bool> UpdateAsync(string id, UpdateEmployeeDto dto)
        {
            var employee = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("employee of given id not found");
            var employeeById = await _repository.GetEmployeeByEmailAsync(dto.Email);
            if (employeeById is not null) throw new InvalidOperationException("Email Already Exists");
            EmployeeMappers.UpdateEmployee(employee, dto);
            await _repository.UpdateAsync(employee);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee is null)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}