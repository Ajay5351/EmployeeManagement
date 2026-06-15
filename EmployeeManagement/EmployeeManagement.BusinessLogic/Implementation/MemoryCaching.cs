using EmployeeManagement.Models;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.BusinessLogic.Implementation
{
    public class MemoryCaching
    {
        private readonly ICacheProvider _cacheProvider;

        public MemoryCaching(ICacheProvider cacheProvider)
        {
           _cacheProvider = cacheProvider;
        }

        public async Task<PagedEmployeeResult> GetAllEmployeesCacheAsync(EmployeeRequestModel requestModel, PagedEmployeeResult pagedResult)
        {
            string cacheKey = $"Employee_{requestModel.Term}_{requestModel.Sort}_{requestModel.Page}_{requestModel.Limit}";

            if (_cacheProvider.TryGetValue(cacheKey, out PagedEmployeeResult? result))
            {
                return result!;
            }

            result = new PagedEmployeeResult
            {
                Employees = pagedResult.Employees,
                TotalCount = pagedResult.TotalCount,
                TotalPages = pagedResult.TotalPages
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30),
                Size = 1000
            };

            _cacheProvider.Set(cacheKey, result, cacheEntryOptions);

            return result;
        }

        public async Task<EmployeeModel?> GetEmployeeByIdCacheAsync(int id, Func<Task<EmployeeModel?>> getEmployee)
        {
            string cacheKey = $"Employee_{id}";

            if (_cacheProvider.TryGetValue(cacheKey, out EmployeeModel? cachedEmployee))
            {
                return cachedEmployee;
            }

            var employee = await getEmployee();

            if (employee != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1000
                };

                _cacheProvider.Set(cacheKey, employee, cacheEntryOptions);
            }

            return employee;
        }
    } 
}
