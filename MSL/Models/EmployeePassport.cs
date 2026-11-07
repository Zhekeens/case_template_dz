using System.ComponentModel.DataAnnotations.Schema;

namespace MSL.Models
{
    public class EmployeePassport
    {
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public string PassportData { get; set; } = string.Empty;
        public Employee? Employee { get; set; }
    }
}
