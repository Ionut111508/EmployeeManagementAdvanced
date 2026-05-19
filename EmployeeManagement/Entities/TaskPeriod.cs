namespace EmployeeManagement.Entities
{
    public class TaskPeriod
    {
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public string PeriodId { get; set; } = null!;

        public TaskItem? TaskItem { get; set; }
        public Period? Period { get; set; }
    }
}
