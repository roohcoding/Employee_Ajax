using System.ComponentModel.DataAnnotations;

namespace EmployeePayrollAjax.Models
{
    public class EmployeeModel
    {

        [Key]
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public string Notes { get; set; }
    }
}
