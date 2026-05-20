namespace EmployeeManagement.DTOs;

public class AutoAllocationRequest
{
    public string ProjectId { get; set; } = null!;
    public string TaskId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HoursPerDay { get; set; }
    public string? SkillId { get; set; }
}
