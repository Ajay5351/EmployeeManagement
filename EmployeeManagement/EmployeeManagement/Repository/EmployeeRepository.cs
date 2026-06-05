using AutoMapper;
using EmployeeManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;
        private readonly IMapper _mapper;


        public EmployeeRepository(EmployeeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return _mapper.Map<List<Employee>>(employees);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return _mapper.Map<Employee?>(employee);
        }

    }
}
