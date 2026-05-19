namespace EmployeeManagement.Entities
{
    public class Period
    {
        public string PeriodId { get; set; } = null!;
        public string PeriodName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation properties
        public ICollection<TaskPeriod> TaskPeriods { get; set; } = new List<TaskPeriod>();
        public ICollection<ProjectPeriod> ProjectPeriods { get; set; } = new List<ProjectPeriod>();
    }
}
