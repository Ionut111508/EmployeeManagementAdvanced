namespace EmployeeManagement.DTOs
{
    public class WorkNormDto
    {
        public string WorkNormId { get; set; } = null!;
        public string WorkNormName { get; set; } = null!;
        public decimal WorkHours { get; set; }
    }

    public class WorkNormCreateDto
    {
        public string WorkNormId { get; set; } = null!;
        public string WorkNormName { get; set; } = null!;
        public decimal WorkHours { get; set; }
    }

    public class WorkNormUpdateDto
    {
        public string WorkNormName { get; set; } = null!;
        public decimal WorkHours { get; set; }
    }
}
