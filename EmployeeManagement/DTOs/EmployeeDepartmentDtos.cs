namespace EmployeeManagement.DTOs;

// Response DTOs for EmployeeDepartment
public class EmployeeDepartmentResponse
{
    public string EmployeeId { get; set; } = null!;
    public string DepartmentId { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EmployeeBasicDto? Employee { get; set; }
    public DepartmentBasicDto? Department { get; set; }
}

public class EmployeeDepartmentByEmployeeResponse
{
    public string EmployeeId { get; set; } = null!;
    public string DepartmentId { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DepartmentBasicDto? Department { get; set; }
}

public class EmployeeDepartmentByDepartmentResponse
{
    public string EmployeeId { get; set; } = null!;
    public string DepartmentId { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EmployeeBasicDto? Employee { get; set; }
    public DepartmentBasicDto? Department { get; set; }
}

// Basic DTO for nesting
public class DepartmentBasicDto
{
    public string DepartmentId { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;
}
