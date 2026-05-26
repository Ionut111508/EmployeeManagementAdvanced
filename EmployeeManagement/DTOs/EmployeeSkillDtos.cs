namespace EmployeeManagement.DTOs;

// Response DTOs for EmployeeSkill
public class EmployeeSkillResponse
{
    public string EmployeeId { get; set; } = null!;
    public string SkillId { get; set; } = null!;
    public DateTime? AcquiredDate { get; set; }
    public EmployeeBasicDto? Employee { get; set; }
    public SkillBasicDto? Skill { get; set; }
}

public class EmployeeSkillByEmployeeResponse
{
    public string EmployeeId { get; set; } = null!;
    public string SkillId { get; set; } = null!;
    public DateTime? AcquiredDate { get; set; }
    public SkillBasicDto? Skill { get; set; }
}

public class EmployeeSkillBySkillResponse
{
    public string EmployeeId { get; set; } = null!;
    public string SkillId { get; set; } = null!;
    public DateTime? AcquiredDate { get; set; }
    public EmployeeBasicDto? Employee { get; set; }
    public SkillBasicDto? Skill { get; set; }
}

// Basic DTOs for nesting
public class EmployeeBasicDto
{
    public string EmployeeId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AccountId { get; set; }
    public string? WorkNormId { get; set; }
}

public class SkillBasicDto
{
    public string SkillId { get; set; } = null!;
    public string SkillName { get; set; } = null!;
    public string? SkillLevel { get; set; }
}
