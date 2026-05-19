namespace EmployeeManagement.Entities
{
    public class Employee
    {
        public string EmployeeId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string WorkNormId { get; set; } = null!;

        public Account? Account { get; set; }
        public WorkNorm? WorkNorm { get; set; }
        public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
        public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = new List<EmployeeDepartment>();
        public ICollection<ProjectManager> ProjectManagers { get; set; } = new List<ProjectManager>();
        public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
        public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
        public ICollection<Period> Periods { get; set; } = new List<Period>();
    }
}
