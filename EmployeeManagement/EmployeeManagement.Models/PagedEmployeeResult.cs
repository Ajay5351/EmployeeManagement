namespace EmployeeManagement.Models
{
    public class PagedEmployeeResult
    {
        public List<EmployeeModel> Employees { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
