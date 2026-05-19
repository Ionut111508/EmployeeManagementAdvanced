namespace EmployeeManagement.Entities
{
    public class Allocation
    {
        public string EmployeeId { get; set; } = null!;
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public int AllocationPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }
        public Project? Project { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
