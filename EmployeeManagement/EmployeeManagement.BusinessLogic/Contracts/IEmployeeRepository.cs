using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.BusinessLogic
{
    public interface IEmployeeRepository
    {
        Task<PagedEmployeeResult> GetAllEmployeesAsync(EmployeeRequestModel requestModel);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int id);
        Task<EmployeeModel> AddEmployeeAsync(EmployeeCreateRequest employee);
        Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel existingEmployee, EmployeeUpdateRequest updatedEmployee);
        Task<EmployeeModel> PatchEmployeeAsync(EmployeeModel existingEmployee, JsonPatchDocument<EmployeeModel> employee);
        Task DeleteEmployeeAsync(EmployeeModel employee);
        Task<bool> IsEmployeeExistsAsync(string? email);
    }
}
