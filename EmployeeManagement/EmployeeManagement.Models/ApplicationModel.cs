using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class ApplicationModel : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string? LastName { get; set; }
    }
}