using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.BusinessLogic.Implementation
{
    public class Pagination
    {
        public async Task<PagedEmployeeResult> ApplyPaginationAsync(IQueryable<EmployeeModel> employees, EmployeeRequestModel requestModel)
        {
            var totalCount = await employees.CountAsync();

            var pagedEmployees = await employees
                .Skip((requestModel.Page - 1) * requestModel.Limit)
                .Take(requestModel.Limit)
                .ToListAsync();

            return new PagedEmployeeResult
            {
                Employees = pagedEmployees,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / requestModel.Limit)
            };
        }
    }
}
