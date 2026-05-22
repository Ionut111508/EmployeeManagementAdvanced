namespace EmployeeManagement.Entities
{
    public class EmployeeLeave
    {
        public string EmployeeLeaveId { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = "Vacation";
        public string? Reason { get; set; }
        public string? ReplacementEmployeeId { get; set; }

        public Employee? Employee { get; set; }
        public Employee? ReplacementEmployee { get; set; }
    }
}
