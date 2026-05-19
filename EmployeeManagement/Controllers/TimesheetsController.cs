using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimesheetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TimesheetsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Timesheets.ToListAsync());
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId)
    {
        var result = await _context.Timesheets
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.WorkDate)
            .ToListAsync();
        return Ok(result);
    }

    [HttpGet("task/{projectId}/{taskId}")]
    public async Task<IActionResult> GetByTask(string projectId, string taskId)
    {
        var result = await _context.Timesheets
            .Where(x => x.ProjectId == projectId && x.TaskId == taskId)
            .OrderByDescending(x => x.WorkDate)
            .ToListAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TimesheetRequest request)
    {
        if (request.WorkedHours <= 0) return BadRequest("Invalid hours.");

        var employeeExists = await _context.Employees.AnyAsync(x => x.EmployeeId == request.EmployeeId);
        if (!employeeExists) return BadRequest("Invalid employee.");

        var taskExists = await _context.TaskItems.AnyAsync(x => x.ProjectId == request.ProjectId && x.TaskId == request.TaskId);
        if (!taskExists) return BadRequest("Invalid task.");

        var workDate = request.WorkDate.Date;
        var existing = await _context.Timesheets.FindAsync(request.ProjectId, request.TaskId, request.EmployeeId, workDate);

        if (existing == null)
        {
            existing = new Timesheet
            {
                ProjectId = request.ProjectId,
                TaskId = request.TaskId,
                EmployeeId = request.EmployeeId,
                WorkDate = workDate,
                WorkedHours = request.WorkedHours
            };
            _context.Timesheets.Add(existing);
        }
        else
        {
            existing.WorkedHours += request.WorkedHours;
        }

        await _context.SaveChangesAsync();
        return Ok(existing);
    }
}
