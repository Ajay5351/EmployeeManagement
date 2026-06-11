using AutoMapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
             return await _context.Employees.ToListAsync();
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

            bool emailExists = await _context.Employees.AnyAsync(x => x.Email == employee.Email
                            && x.Id != employee.Id);

            if (emailExists)
                throw new Exception("Email already exists.");

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