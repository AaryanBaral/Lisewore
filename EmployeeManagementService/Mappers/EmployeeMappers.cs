using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementService.Dtos;
using EmployeeManagementService.Models;

namespace EmployeeManagementService.Mappers
{
    public static class EmployeeMappers
    {
        public static Employee ToEmployee(CreateEmployeeDto dto)
        {
            return new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Department = dto.Department,
                HireDate = dto.HireDate
            };
        }

        public static void UpdateEmployee(Employee employee, UpdateEmployeeDto dto)
        {
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.Department = dto.Department;
            employee.HireDate = dto.HireDate;
        }

        public static ReadEmployeeDto ToReadDto(Employee employee)
        {
            return new ReadEmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department,
                HireDate = employee.HireDate
            };
        }
    }
}