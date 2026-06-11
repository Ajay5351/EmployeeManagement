namespace EmployeeManagement.Models
{
    public class PagedEmployeeResult
    {
        public List<Employee> Employees { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
