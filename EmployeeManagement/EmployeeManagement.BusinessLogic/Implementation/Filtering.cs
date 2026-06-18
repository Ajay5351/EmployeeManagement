using EmployeeManagement.Models;

namespace EmployeeManagement.BusinessLogic.Implementation
{
    public class Filtering
    {
        public IQueryable<EmployeeModel> ApplyFiltering(IQueryable<EmployeeModel> employees, string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return employees;
            }

            term = term.ToLower();
            return employees.Where(e =>
                e.Name.ToLower().Contains(term) ||
                e.Department.ToLower().Contains(term) ||
                e.Salary.ToString().Contains(term));
        }
    }
}
