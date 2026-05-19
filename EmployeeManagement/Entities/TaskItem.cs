namespace EmployeeManagement.Entities
{
    public class TaskItem
    {
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public string TaskName { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EstimatedHours { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();
        public ICollection<Allocation> Allocations { get; set; } = new List<Allocation>();
        public ICollection<Timesheet> Timesheets { get; set; } = new List<Timesheet>();
        public ICollection<TaskPeriod> TaskPeriods { get; set; } = new List<TaskPeriod>();
    }
}
