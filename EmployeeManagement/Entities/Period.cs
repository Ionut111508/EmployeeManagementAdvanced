namespace EmployeeManagement.Entities
{
    public class Period
    {
        public string PeriodId { get; set; } = null!;
        public string? Year { get; set; }
        public string? Month { get; set; }
        public string? Day { get; set; }
        public string PeriodType { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;

        public Employee? Employee { get; set; }
        public ICollection<TaskPeriod> TaskPeriods { get; set; } = new List<TaskPeriod>();
        public ICollection<ProjectPeriod> ProjectPeriods { get; set; } = new List<ProjectPeriod>();
    }
}
