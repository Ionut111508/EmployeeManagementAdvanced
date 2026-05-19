using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectManagersController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectManagersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(string projectId)
    {
        var result = await _context.ProjectManagers
            .Where(x => x.ProjectId == projectId)
            .Include(x => x.Employee)
            .ToListAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProjectManagerRequest request)
    {
        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == request.EmployeeId)) return BadRequest("Invalid employee.");
        if (!await _context.Projects.AnyAsync(x => x.ProjectId == request.ProjectId)) return BadRequest("Invalid project.");

        var exists = await _context.ProjectManagers.FindAsync(request.EmployeeId, request.ProjectId);
        if (exists != null) return BadRequest("Duplicate project manager.");

        var item = new ProjectManager
        {
            EmployeeId = request.EmployeeId,
            ProjectId = request.ProjectId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.ProjectManagers.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{employeeId}/{projectId}")]
    public async Task<IActionResult> Delete(string employeeId, string projectId)
    {
        var item = await _context.ProjectManagers.FindAsync(employeeId, projectId);
        if (item == null) return NotFound();
        _context.ProjectManagers.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
