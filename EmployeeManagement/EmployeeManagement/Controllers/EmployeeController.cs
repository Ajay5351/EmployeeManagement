using EmployeeManagement.BusinessLogic;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync([FromBody] EmployeeRequestModel employeeRequest)
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync(employeeRequest);

            if (employees == null || employees.Employees.Count == 0)
            {
                return NotFound("No employees found.");
            }

            Response.Headers.Append("X-Total-Count", employees.Employees.Count().ToString());
            Response.Headers.Append("X-Total-Pages", employees.TotalPages.ToString());

            return Ok(employees.Employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound($"Employee with Id {id} not found.");
            }

            return Ok(employee);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeCreateRequest employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isEmployeeExists = await _employeeRepository.IsEmployeeExistsAsync(employee.Email);

            if (isEmployeeExists)
                return Conflict($"An employee with email {employee.Email} already exists.");

            var createdEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            return Ok(createdEmployee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] int id, [FromBody] EmployeeUpdateRequest employee)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id {id} not found.");
            }

            var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(existingEmployee, employee);
            return Ok(updatedEmployee);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployeeAsync(int id, [FromBody] JsonPatchDocument<EmployeeModel> employee)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id {id} not found.");
            }

            var patchedEmployee = await _employeeRepository.PatchEmployeeAsync(existingEmployee, employee);

            return Ok(patchedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeByIdAsync([FromRoute] int id)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id {id} not found.");
            }

            await _employeeRepository.DeleteEmployeeAsync(existingEmployee);
            return Ok($"Employee with Id {id} deleted successfully.");
        }
    }
}