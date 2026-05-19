using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeSkillsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeSkillsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId)
    {
        var result = await _context.EmployeeSkills
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Skill)
            .ToListAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeSkillRequest request)
    {
        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == request.EmployeeId)) return BadRequest("Invalid employee.");
        if (!await _context.Skills.AnyAsync(x => x.SkillId == request.SkillId)) return BadRequest("Invalid skill.");

        var exists = await _context.EmployeeSkills.FindAsync(request.EmployeeId, request.SkillId);
        if (exists != null) return BadRequest("Duplicate employee skill.");

        var item = new EmployeeSkill
        {
            EmployeeId = request.EmployeeId,
            SkillId = request.SkillId,
            AcquiredDate = request.AcquiredDate ?? DateTime.Today
        };

        _context.EmployeeSkills.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{employeeId}/{skillId}")]
    public async Task<IActionResult> Delete(string employeeId, string skillId)
    {
        var item = await _context.EmployeeSkills.FindAsync(employeeId, skillId);
        if (item == null) return NotFound();
        _context.EmployeeSkills.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
