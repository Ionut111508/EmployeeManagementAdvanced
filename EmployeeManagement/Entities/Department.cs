namespace EmployeeManagement.Entities
{
    public class Department
    {
        public string DepartmentId { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
    }
}
