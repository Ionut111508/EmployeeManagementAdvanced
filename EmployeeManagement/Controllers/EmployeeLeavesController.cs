using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeLeavesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeLeavesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
SELECT l.EmployeeLeaveId, l.EmployeeId, e.FirstName + ' ' + e.LastName AS EmployeeName, l.StartDate, l.EndDate, l.LeaveType, l.Reason, l.ReplacementEmployeeId,
       CASE WHEN r.EmployeeId IS NULL THEN NULL ELSE r.FirstName + ' ' + r.LastName END AS ReplacementEmployeeName
FROM EmployeeLeave l
JOIN Employee e ON e.EmployeeId = l.EmployeeId
LEFT JOIN Employee r ON r.EmployeeId = l.ReplacementEmployeeId
ORDER BY l.StartDate";
        if (connection.State != System.Data.ConnectionState.Open) await connection.OpenAsync();
        var result = new List<EmployeeLeaveDto>();
        try
        {
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync()) result.Add(ReadLeave(reader));
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return Ok(result);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeLeaveCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.EmployeeId)) return BadRequest("Employee is required.");
        if (dto.StartDate.Date > dto.EndDate.Date) return BadRequest("End date cannot be before start date.");
        if (!await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId)) return BadRequest("Employee does not exist.");
        if (!string.IsNullOrWhiteSpace(dto.ReplacementEmployeeId) && !await _context.Employees.AnyAsync(e => e.EmployeeId == dto.ReplacementEmployeeId)) return BadRequest("Replacement employee does not exist.");
        if (dto.ReplacementEmployeeId == dto.EmployeeId) return BadRequest("Replacement cannot be the same employee.");

        var id = string.IsNullOrWhiteSpace(dto.EmployeeLeaveId) ? "LV" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant() : dto.EmployeeLeaveId;
        var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
IF EXISTS (SELECT 1 FROM EmployeeLeave WHERE EmployeeId = @EmployeeId AND StartDate <= @EndDate AND EndDate >= @StartDate)
BEGIN
    SELECT 'OVERLAP';
END
ELSE
BEGIN
    INSERT INTO EmployeeLeave (EmployeeLeaveId, EmployeeId, StartDate, EndDate, LeaveType, Reason, ReplacementEmployeeId)
    VALUES (@Id, @EmployeeId, @StartDate, @EndDate, @LeaveType, @Reason, @ReplacementEmployeeId);
    SELECT 'OK';
END";
        Add(command, "@Id", id);
        Add(command, "@EmployeeId", dto.EmployeeId);
        Add(command, "@StartDate", dto.StartDate.Date);
        Add(command, "@EndDate", dto.EndDate.Date);
        Add(command, "@LeaveType", string.IsNullOrWhiteSpace(dto.LeaveType) ? "Vacation" : dto.LeaveType);
        Add(command, "@Reason", dto.Reason ?? (object)DBNull.Value);
        Add(command, "@ReplacementEmployeeId", string.IsNullOrWhiteSpace(dto.ReplacementEmployeeId) ? DBNull.Value : dto.ReplacementEmployeeId);
        if (connection.State != System.Data.ConnectionState.Open) await connection.OpenAsync();
        try
        {
            var status = (await command.ExecuteScalarAsync())?.ToString();
            if (status == "OVERLAP") return BadRequest("Employee already has leave in this period.");
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return BadRequest("EmployeeLeave table does not exist in the database.");
        }
        return Ok(new { employeeLeaveId = id });
    }

    [HttpGet("{leaveId}/impact")]
    public async Task<IActionResult> GetImpact(string leaveId)
    {
        var connection = _context.Database.GetDbConnection();
        await using var command = connection.CreateCommand();
        command.CommandText = @"
SELECT a.ProjectId, p.ProjectName, a.TaskId, t.TaskName, a.AllocationStartDate, a.AllocationEndDate, a.HoursPerDay,
       CASE WHEN l.ReplacementEmployeeId IS NULL THEN 'Needs replacement or delay' ELSE 'Replacement selected' END AS Status
FROM EmployeeLeave l
JOIN Allocation a ON a.EmployeeId = l.EmployeeId AND a.AllocationStartDate <= l.EndDate AND ISNULL(a.AllocationEndDate, a.AllocationStartDate) >= l.StartDate
JOIN TaskItem t ON t.ProjectId = a.ProjectId AND t.TaskId = a.TaskId
JOIN Project p ON p.ProjectId = a.ProjectId
WHERE l.EmployeeLeaveId = @LeaveId";
        Add(command, "@LeaveId", leaveId);
        if (connection.State != System.Data.ConnectionState.Open) await connection.OpenAsync();
        var result = new List<object>();
        try
        {
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new
                {
                    ProjectId = reader.GetString(0),
                    ProjectName = reader.GetString(1),
                    TaskId = reader.GetString(2),
                    TaskName = reader.GetString(3),
                    AllocationStartDate = reader.GetDateTime(4),
                    AllocationEndDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                    AllocatedHours = reader.GetDecimal(6),
                    Status = reader.GetString(7)
                });
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return Ok(result);
        }
        return Ok(result);
    }

    private static EmployeeLeaveDto ReadLeave(System.Data.Common.DbDataReader reader) => new()
    {
        EmployeeLeaveId = reader.GetString(0),
        EmployeeId = reader.GetString(1),
        EmployeeName = reader.GetString(2),
        StartDate = reader.GetDateTime(3),
        EndDate = reader.GetDateTime(4),
        LeaveType = reader.GetString(5),
        Reason = reader.IsDBNull(6) ? null : reader.GetString(6),
        ReplacementEmployeeId = reader.IsDBNull(7) ? null : reader.GetString(7),
        ReplacementEmployeeName = reader.IsDBNull(8) ? null : reader.GetString(8)
    };

    private static void Add(System.Data.Common.DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
