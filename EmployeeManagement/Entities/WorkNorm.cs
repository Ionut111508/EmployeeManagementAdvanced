namespace EmployeeManagement.Entities
{
    public class WorkNorm
    {
        public string WorkNormId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int HoursPerDay { get; set; }
        public int DaysPerWeek { get; set; }
    }
}
