namespace EmployeeManagement.Entities
{
    public class TaskPeriod
    {
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public string PeriodId { get; set; } = null!;
        public int? PlannedHours { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public TaskItem? TaskItem { get; set; }
        public Period? Period { get; set; }
    }
}
