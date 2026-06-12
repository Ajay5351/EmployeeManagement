using EmployeeManagement.API.Caching;
using EmployeeManagement.Models;
using EmployeeManagement.BusinessLogic;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICacheProvider _cacheProvider;

        public EmployeeController(IEmployeeRepository employeeRepository, ICacheProvider cacheProvider)
        {
            _employeeRepository = employeeRepository;
            _cacheProvider = cacheProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees([FromQuery] string? term, [FromQuery] string? sort,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 5)
        {
            string cacheKey = $"Employee_{term}_{sort}_{page}_{limit}";

            if (!_cacheProvider.TryGetValue(cacheKey, out PagedEmployeeResult? result))
            {
                result = await _employeeRepository.GetAllEmployees(term, sort, page, limit);

                if (result == null)
                {
                    return NotFound("No employees found.");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1000
                };

                _cacheProvider.Set(cacheKey, result, cacheEntryOptions);
            }

            Response.Headers.Append("X-Total-Count", result!.TotalCount.ToString());
            Response.Headers.Append("X-Total-Pages", result.TotalPages.ToString());

            return Ok(result.Employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            if (!_cacheProvider.TryGetValue(CacheKeys.Employee, out Employee? employee))
            {
                employee = await _employeeRepository.GetEmployeeById(id);

                if (employee == null)
                {
                    return NotFound($"Employee with Id {id} not found.");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1000
                };

                _cacheProvider.Set(CacheKeys.Employee, employee, cacheEntryOptions);
            }

            return Ok(employee);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result =
                    await _employeeRepository.AddEmployee(employee);

                return CreatedAtAction(
                    nameof(GetEmployeeById),
                    new { id = result.Id },
                    result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employee.Id = id;

            try
            {
                var updatedEmployee = await _employeeRepository.UpdateEmployee(employee);

                return Ok(updatedEmployee);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the employee: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeById([FromRoute] int id)
        {
            try
            {
                await _employeeRepository.DeleteEmployee(id);
                return NoContent();
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}