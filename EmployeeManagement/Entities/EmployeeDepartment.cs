namespace EmployeeManagement.Entities
{
    public class EmployeeDepartment
    {
        public string EmployeeId { get; set; } = null!;
        public string DepartmentId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Employee? Employee { get; set; }
        public Department? Department { get; set; }
    }
}
