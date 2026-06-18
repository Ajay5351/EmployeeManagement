using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }

        public int Salary { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Location { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Email { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Department { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string? Qualification { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
