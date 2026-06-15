using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagement.BusinessLogic
{
    public interface IEmployeeRepository
    {
        Task<PagedEmployeeResult> GetAllEmployeesAsync(EmployeeRequestModel requestModel);
        Task<EmployeeModel?> GetEmployeeByIdAsync(int id);
        Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee);
        Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee);
        Task<EmployeeModel> PatchEmployeeAsync(int id, JsonPatchDocument<EmployeeModel> employee);
        Task DeleteEmployeeAsync(int id);
    }
}
