using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeDbContext context, Filtering filtering, Sorting sorting, Pagination pagination, MemoryCaching memoryCaching, IMapper mapper)
        {
            _context = context;
            _filtering = filtering;
            _sorting = sorting;
            _pagination = pagination;
            _memoryCaching = memoryCaching;
            _mapper = mapper;
        }

        public async Task<PagedEmployeeResult> GetAllEmployeesAsync(EmployeeRequestModel employeeRequest)
        {
            var cachedResult = _memoryCaching.GetAllEmployeesCache(employeeRequest);

            if (cachedResult != null)
            {
                return cachedResult;
            }

            IQueryable<EmployeeModel> employees = _context.Employees.AsNoTracking();

            employees = _filtering.ApplyFiltering(employees, employeeRequest.Term);

            employees = _sorting.ApplySorting(employees, employeeRequest.Sort);

            var pagedEmployees = await _pagination.ApplyPaginationAsync(employees, employeeRequest);

            _memoryCaching.SetEmployeesCache(employeeRequest, pagedEmployees);

            return pagedEmployees;
        }

        public async Task<EmployeeModel?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _memoryCaching.GetEmployeeByIdCacheAsync(id,
                async () => await _context.Employees.FindAsync(id));

            return employee;
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeCreateRequest createdEmployee)
        {
            var employee = _mapper.Map<EmployeeModel>(createdEmployee);
            employee.CreatedDate = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel existingEmployee, EmployeeUpdateRequest updatedEmployee)
        {
            _mapper.Map(updatedEmployee, existingEmployee);

            existingEmployee.UpdatedDate = DateTime.UtcNow;

            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<EmployeeModel> PatchEmployeeAsync(EmployeeModel existingEmployee, JsonPatchDocument<EmployeeModel> employee)
        {
            existingEmployee.UpdatedDate = DateTime.UtcNow;
            employee.ApplyTo(existingEmployee);

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task DeleteEmployeeAsync(EmployeeModel employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsEmployeeExistsAsync(string? email)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }
    }
}
