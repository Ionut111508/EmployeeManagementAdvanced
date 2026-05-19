namespace EmployeeManagement.Entities
{
    public class Timesheet
    {
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public DateTime EntryDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public TaskItem? TaskItem { get; set; }
        public Employee? Employee { get; set; }
    }
}
