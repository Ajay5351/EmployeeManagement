using EmployeeManagement.Models;

namespace EmployeeManagement.BusinessLogic.Implementation
{
    public class Sorting
    {
        public IQueryable<EmployeeModel> ApplySorting(IQueryable<EmployeeModel> employees, string? sort)
        {
            return sort?.ToLower() switch
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
    }
}
