namespace EmployeeManagement.DTOs
{
    public class EmployeeLeaveDto
    {
        public string EmployeeLeaveId { get; set; } = null!;
        public string EmployeeId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = null!;
        public string? Reason { get; set; }
        public string? ReplacementEmployeeId { get; set; }
        public string? ReplacementEmployeeName { get; set; }
    }

    public class EmployeeLeaveCreateDto
    {
        public string? EmployeeLeaveId { get; set; }
        public string EmployeeId { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; } = "Vacation";
        public string? Reason { get; set; }
        public string? ReplacementEmployeeId { get; set; }
    }
}
