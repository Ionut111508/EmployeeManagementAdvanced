namespace EmployeeManagement.Entities
{
    public class Project
    {
        public string ProjectId { get; set; } = null!;
        public string ProjectName { get; set; } = null!;

        // Navigation properties
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
        public ICollection<ProjectManager> ProjectManagers { get; set; } = new List<ProjectManager>();
        public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
        public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
        public ICollection<TaskPeriod> TaskPeriods { get; set; } = new List<TaskPeriod>();
        public ICollection<ProjectPeriod> ProjectPeriods { get; set; } = new List<ProjectPeriod>();
    }
}