using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeWebApi.Models
{
    public class EmployeeInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }

        public bool IsParmanent { get; set; }

        public double Salary { get; set; }
        public string? ImgPath { get; set; }
    }
}
