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
            .Select(x => new EmployeeSkillResponse
            {
                EmployeeId = x.EmployeeId,
                SkillId = x.SkillId,
                AcquiredDate = x.AcquiredDate,
                Employee = x.Employee != null ? new EmployeeBasicDto
                {
                    EmployeeId = x.Employee.EmployeeId,
                    FirstName = x.Employee.FirstName,
                    LastName = x.Employee.LastName,
                    Email = x.Employee.Email,
                    PhoneNumber = x.Employee.PhoneNumber,
                    AccountId = x.Employee.AccountId,
                    WorkNormId = x.Employee.WorkNormId
                } : null,
                Skill = x.Skill != null ? new SkillBasicDto
                {
                    SkillId = x.Skill.SkillId,
                    SkillName = x.Skill.SkillName,
                    SkillLevel = x.Skill.SkillLevel
                } : null
            })
            .ToListAsync();
        return Ok(result);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId)
    {
        if (!await _context.Employees.AsNoTracking().AnyAsync(x => x.EmployeeId == employeeId))
            return NotFound("Employee not found.");

        var result = await _context.EmployeeSkills
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => new EmployeeSkillByEmployeeResponse
            {
                EmployeeId = x.EmployeeId,
                SkillId = x.SkillId,
                AcquiredDate = x.AcquiredDate,
                Skill = x.Skill != null ? new SkillBasicDto
                {
                    SkillId = x.Skill.SkillId,
                    SkillName = x.Skill.SkillName,
                    SkillLevel = x.Skill.SkillLevel
                } : null
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("skill/{skillId}")]
    public async Task<IActionResult> GetBySkill(string skillId)
    {
        if (!await _context.Skills.AsNoTracking().AnyAsync(x => x.SkillId == skillId))
            return NotFound("Skill not found.");

        var result = await _context.EmployeeSkills
            .AsNoTracking()
            .Where(x => x.SkillId == skillId)
            .Select(x => new EmployeeSkillBySkillResponse
            {
                EmployeeId = x.EmployeeId,
                SkillId = x.SkillId,
                AcquiredDate = x.AcquiredDate,
                Employee = x.Employee != null ? new EmployeeBasicDto
                {
                    EmployeeId = x.Employee.EmployeeId,
                    FirstName = x.Employee.FirstName,
                    LastName = x.Employee.LastName,
                    Email = x.Employee.Email,
                    PhoneNumber = x.Employee.PhoneNumber,
                    AccountId = x.Employee.AccountId,
                    WorkNormId = x.Employee.WorkNormId
                } : null,
                Skill = x.Skill != null ? new SkillBasicDto
                {
                    SkillId = x.Skill.SkillId,
                    SkillName = x.Skill.SkillName,
                    SkillLevel = x.Skill.SkillLevel
                } : null
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeSkillRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.EmployeeId) || string.IsNullOrWhiteSpace(request.SkillId))
            return BadRequest("EmployeeId and SkillId are required.");

        if (!await _context.Employees.AsNoTracking().AnyAsync(x => x.EmployeeId == request.EmployeeId))
            return BadRequest("Selected employee does not exist.");

        if (!await _context.Skills.AsNoTracking().AnyAsync(x => x.SkillId == request.SkillId))
            return BadRequest("Selected skill does not exist.");

        var exists = await _context.EmployeeSkills.AsNoTracking().AnyAsync(x => x.EmployeeId == request.EmployeeId && x.SkillId == request.SkillId);
        if (exists)
            return BadRequest("Employee already has this skill.");

        var item = new EmployeeSkill
        {
            EmployeeId = request.EmployeeId,
            SkillId = request.SkillId,
            AcquiredDate = request.AcquiredDate ?? DateTime.Today
        };

        _context.EmployeeSkills.Add(item);
        await _context.SaveChangesAsync();

        var response = new EmployeeSkillResponse
        {
            EmployeeId = item.EmployeeId,
            SkillId = item.SkillId,
            AcquiredDate = item.AcquiredDate
        };

        return CreatedAtAction(nameof(GetByEmployee), new { employeeId = item.EmployeeId }, response);
    }

    [HttpDelete("{employeeId}/{skillId}")]
    public async Task<IActionResult> Delete(string employeeId, string skillId)
    {
        var item = await _context.EmployeeSkills.FindAsync(employeeId, skillId);
        if (item == null)
            return NotFound("Employee skill assignment not found.");

        _context.EmployeeSkills.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
