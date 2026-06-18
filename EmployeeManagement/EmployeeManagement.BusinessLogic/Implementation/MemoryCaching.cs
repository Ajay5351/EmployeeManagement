using EmployeeManagement.Models;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace EmployeeManagement.BusinessLogic.Implementation
{
    public class MemoryCaching
    {
        private readonly ICacheProvider _cacheProvider;

        private readonly MemoryCacheEntryOptions _cacheOptions =
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(30)
            };

        public MemoryCaching(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public PagedEmployeeResult? GetAllEmployeesCache(EmployeeRequestModel request)
        {
            string cacheKey = $"Employee_{request.Term}_{request.Sort}_{request.Page}_{request.Limit}";

            _cacheProvider.TryGetValue(cacheKey, out PagedEmployeeResult? result);
            return result;
        }

        public void SetEmployeesCache(EmployeeRequestModel request, PagedEmployeeResult result)
        {
            string cacheKey = $"Employee_{request.Term}_{request.Sort}_{request.Page}_{request.Limit}";

            _cacheProvider.Set(cacheKey, result, _cacheOptions);
        }

        public async Task<EmployeeModel?> GetEmployeeByIdCacheAsync(int id, Func<Task<EmployeeModel?>> getEmployee)
        {
            string cacheKey = $"Employee_{id}";

            if (_cacheProvider.TryGetValue(cacheKey, out EmployeeModel? employee))
            {
                return employee;
            }

            employee = await getEmployee();

            if (employee != null)
            {
                _cacheProvider.Set(cacheKey, employee, _cacheOptions);
            }
            return employee;
        }

        public void RemoveEmployeeCache(int id)
        {
            _cacheProvider.Remove($"Employee_{id}");
        }
    }
}