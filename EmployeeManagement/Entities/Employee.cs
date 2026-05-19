namespace EmployeeManagement.Entities
{
    public class Employee
    {
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string? AccountId { get; set; }
        public string? WorkNormId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime HireDate { get; set; }

        // Navigation properties
        public Account? Account { get; set; }
        public WorkNorm? WorkNorm { get; set; }
        public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
        public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
        public ICollection<ProjectManager> ProjectManagers { get; set; } = new List<ProjectManager>();
        public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
        public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
    }
}
