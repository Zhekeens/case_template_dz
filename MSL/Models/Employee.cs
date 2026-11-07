using System.ComponentModel.DataAnnotations.Schema;

namespace MSL.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public EmployeePassport? Passport { get; set; }
        public int ClearanceLevel { get; set; }
    }
}
