namespace EmployeeManagement.Entities
{
    public class ProjectPeriod
    {
        public string ProjectId { get; set; } = null!;
        public string PeriodId { get; set; } = null!;
        public string? Status { get; set; }
        public int? BudgetHours { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public Period? Period { get; set; }
    }
}
