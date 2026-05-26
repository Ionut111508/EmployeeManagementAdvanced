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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _context.EmployeeSkills
            .AsNoTracking()
            .Select(x => new
            {
                x.EmployeeId,
                x.SkillId,
                x.AcquiredDate,
                employee = new
                {
                    x.Employee.EmployeeId,
                    x.Employee.FirstName,
                    x.Employee.LastName,
                    x.Employee.Email,
                    x.Employee.PhoneNumber,
                    x.Employee.AccountId,
                    x.Employee.WorkNormId
                },
                skill = new
                {
                    x.Skill.SkillId,
                    x.Skill.SkillName,
                    x.Skill.SkillLevel
                }
            })
            .ToListAsync();
        return Ok(result);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId)
    {
        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == employeeId))
            return NotFound("Employee was not found.");

        var result = await _context.EmployeeSkills
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => new
            {
                x.EmployeeId,
                x.SkillId,
                x.AcquiredDate,
                skill = new
                {
                    x.Skill.SkillId,
                    x.Skill.SkillName,
                    x.Skill.SkillLevel
                }
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("skill/{skillId}")]
    public async Task<IActionResult> GetBySkill(string skillId)
    {
        if (!await _context.Skills.AnyAsync(x => x.SkillId == skillId))
            return NotFound("Skill was not found.");

        var result = await _context.EmployeeSkills
            .AsNoTracking()
            .Where(x => x.SkillId == skillId)
            .Select(x => new
            {
                x.EmployeeId,
                x.SkillId,
                x.AcquiredDate,
                employee = new
                {
                    x.Employee.EmployeeId,
                    x.Employee.FirstName,
                    x.Employee.LastName,
                    x.Employee.Email,
                    x.Employee.PhoneNumber,
                    x.Employee.AccountId,
                    x.Employee.WorkNormId
                },
                skill = new
                {
                    x.Skill.SkillId,
                    x.Skill.SkillName,
                    x.Skill.SkillLevel
                }
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeSkillRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.EmployeeId) || string.IsNullOrWhiteSpace(request.SkillId))
            return BadRequest("EmployeeId and SkillId are required.");

        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == request.EmployeeId))
            return BadRequest("Selected employee does not exist.");

        if (!await _context.Skills.AnyAsync(x => x.SkillId == request.SkillId))
            return BadRequest("Selected skill does not exist.");

        var exists = await _context.EmployeeSkills.FindAsync(request.EmployeeId, request.SkillId);
        if (exists != null)
            return BadRequest("Employee already has this skill.");

        var item = new EmployeeSkill
        {
            EmployeeId = request.EmployeeId,
            SkillId = request.SkillId,
            AcquiredDate = request.AcquiredDate ?? DateTime.Today
        };

        _context.EmployeeSkills.Add(item);
        await _context.SaveChangesAsync();
        return Ok(new { item.EmployeeId, item.SkillId, item.AcquiredDate });
    }

    [HttpDelete("{employeeId}/{skillId}")]
    public async Task<IActionResult> Delete(string employeeId, string skillId)
    {
        var item = await _context.EmployeeSkills.FindAsync(employeeId, skillId);
        if (item == null) return NotFound("Employee skill assignment was not found.");

        _context.EmployeeSkills.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
