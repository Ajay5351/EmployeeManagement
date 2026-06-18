namespace EmployeeManagement.Models
{
    public class EmployeeUpdateRequest
    {
        public string? Name { get; set; }

        public int? Salary { get; set; }

        public string? Location { get; set; }

        public string? Email { get; set; }

        public string? Department { get; set; }

        public string? Qualification { get; set; }
    }
}
