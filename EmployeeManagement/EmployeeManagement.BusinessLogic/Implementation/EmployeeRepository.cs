using EmployeeManagement.BusinessLogic.Implementation;
using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.BusinessLogic
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;
        private readonly Filtering _filtering;
        private readonly Sorting _sorting;
        private readonly Pagination _pagination;
        private readonly MemoryCaching _memoryCaching;

        public EmployeeRepository(EmployeeDbContext context, Filtering filtering, Sorting sorting, Pagination pagination, MemoryCaching memoryCaching)
        {
            _context = context;
            _filtering = filtering;
            _sorting = sorting;
            _pagination = pagination;
            _memoryCaching = memoryCaching;
        }

        public async Task<PagedEmployeeResult> GetAllEmployeesAsync(EmployeeRequestModel requestModel)
        {
            IQueryable<EmployeeModel> employees = _context.Employees;

            employees = _filtering.ApplyFiltering(employees, requestModel.Term);

            employees = _sorting.ApplySorting(employees, requestModel.Sort);

            var pagedResult = await _pagination.ApplyPaginationAsync(employees, requestModel);

            return await _memoryCaching.GetAllEmployeesCacheAsync(requestModel, pagedResult);
        }

        public async Task<EmployeeModel?> GetEmployeeByIdAsync(int id)
        {
            return await _memoryCaching.GetEmployeeByIdCacheAsync(id,
                async () => await _context.Employees.FindAsync(id));
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee)
        {
            bool emailExists = await _context.Employees.AnyAsync(x => x.Email == employee.Email);

            if (emailExists)
                throw new Exception("Email already exists.");

            employee.CreatedDate = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee)
        {
            employee.UpdatedDate = DateTime.UtcNow;

            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<EmployeeModel> PatchEmployeeAsync(int id, JsonPatchDocument<EmployeeModel> employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            employee.ApplyTo(existingEmployee);

            existingEmployee.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
