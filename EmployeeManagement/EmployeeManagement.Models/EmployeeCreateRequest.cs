using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeCreateRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Salary must be greater than 0")]
        public int Salary { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        public string? Qualification { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
