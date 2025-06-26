using EmployeeManagementService.Dtos;
using EmployeeManagementService.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        private readonly IEmployeeService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _service.GetAllAsync();
            return Ok(new ResponseDto<List<ReadEmployeeDto>>
            {
                Data = employees
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee is null)
            {
                return NotFound(new ResponseDto<string>
                {
                    Data = $"Employee with ID {id} not found"
                });
            }

            return Ok(new ResponseDto<ReadEmployeeDto>
            {
                Data = employee
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            var id = await _service.AddAsync(dto);
            return Ok(new ResponseDto<string>
            {
                Data = id
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            var success = await _service.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound(new ResponseDto<string>
                {
                    Data = $"Employee with ID {id} not found"
                });
            }

            return Ok(new ResponseDto<string>
            {
                Data = "Employee successfully updated"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new ResponseDto<string>
                {
                    Data = $"Employee with ID {id} not found"
                });
            }

            return Ok(new ResponseDto<string>
            {
                Data = "Employee successfully deleted"
            });
        }
    }
}
