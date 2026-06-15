using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Name should be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1000, 1000000,
            ErrorMessage = "Salary must be between 1000 and 1000000")]
        public float Salary { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, MinimumLength = 3)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Department { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Qualification { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
