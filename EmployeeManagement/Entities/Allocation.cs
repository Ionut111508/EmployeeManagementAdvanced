namespace EmployeeManagement.Entities
{
    public class Allocation
    {
        public string EmployeeId { get; set; } = null!;
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public DateTime AllocationStartDate { get; set; }
        public DateTime? AllocationEndDate { get; set; }
        public decimal AllocatedHours { get; set; }

        public Employee? Employee { get; set; }
        public Project? Project { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
