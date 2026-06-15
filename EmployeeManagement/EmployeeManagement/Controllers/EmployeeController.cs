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
        public async Task<IActionResult> GetAllEmployeesAsync([FromBody] EmployeeRequestModel requestModel)
        {
            try
            {
                var result = await _employeeRepository.GetAllEmployeesAsync(requestModel);

                if (result == null || !result.Employees.Any())
                {
                    return NotFound("No employees found.");
                }

                Response.Headers.Append(
                    "X-Total-Count",
                    result.TotalCount.ToString());

                Response.Headers.Append(
                    "X-Total-Pages",
                    result.TotalPages.ToString());

                return Ok(result.Employees);
            }

            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "An error occurred while retrieving employees.",
                        Error = ex.Message
                    });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    return NotFound($"Employee with Id {id} not found.");
                }

                return Ok(employee);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = "An error occurred while retrieving employee details.",
                        Error = ex.Message
                    });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result =
                    await _employeeRepository.AddEmployeeAsync(employee);

                return CreatedAtAction(
                    nameof(GetEmployeeByIdAsync),
                    new { id = result.Id },
                    result);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] int id, [FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employee.Id = id;

            try
            {
                var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);

                return Ok(updatedEmployee);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the employee: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployeeAsync(int id, [FromBody] JsonPatchDocument<EmployeeModel> employee)
        {
            try
            {
                var updatedEmployee = await _employeeRepository.PatchEmployeeAsync(id, employee);

                return Ok(updatedEmployee);
            }

            catch (NullReferenceException)
            {
                return NotFound(new
                {
                    Message = $"Employee with Id {id} not found."
                });
            }

            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeByIdAsync([FromRoute] int id)
        {
            try
            {
                await _employeeRepository.DeleteEmployeeAsync(id);
                return NoContent();
            }

            catch (NullReferenceException)
            {
                return NotFound(new
                {
                    Message = $"Employee with Id {id} not found."
                });
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}