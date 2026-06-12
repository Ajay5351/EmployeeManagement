using EmployeeManagement.DataAccess;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.BusinessLogic
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<PagedEmployeeResult> GetAllEmployees(string? term, string? sort, int page, int limit)
        {
            IQueryable<Employee> employees;

            if (string.IsNullOrWhiteSpace(term))
            {
                employees = _context.Employees;
            }
            else
            {
                term = term.Trim().ToLower();

                employees = _context.Employees.Where(employee =>
                    employee.Name.ToLower().Contains(term) ||
                    employee.Department.ToLower().Contains(term) ||
                    employee.Location.ToLower().Contains(term));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sort))
            {
                employees = sort.ToLower() switch
                {
                    "name" => employees.OrderBy(e => e.Name),
                    "-name" => employees.OrderByDescending(e => e.Name),

                    "salary" => employees.OrderBy(e => e.Salary),
                    "-salary" => employees.OrderByDescending(e => e.Salary),

                    "department" => employees.OrderBy(e => e.Department),
                    "-department" => employees.OrderByDescending(e => e.Department),

                    _ => employees.OrderBy(e => e.Id)
                };
            }
            else
            {
                employees = employees.OrderBy(e => e.Id);
            }

            // Pagination
            var totalCount = await employees.CountAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)limit);

            var pagedEmployees = await employees.Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return new PagedEmployeeResult
            {
                Employees = pagedEmployees,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            bool emailExists = await _context.Employees.AnyAsync(x => x.Email == employee.Email);

            if (emailExists)
                throw new Exception("Email already exists.");

            employee.CreatedDate = DateTime.UtcNow;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);
           
            existingEmployee.Name = employee.Name;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Location = employee.Location;
            existingEmployee.Email = employee.Email;
            existingEmployee.Department = employee.Department;
            existingEmployee.Qualification = employee.Qualification;
            existingEmployee.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                throw new Exception("Employee not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
