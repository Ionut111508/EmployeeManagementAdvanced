using EmployeeManagement.Data;
using EmployeeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeriodsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PeriodsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Periods.ToListAsync());
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId)
    {
        return Ok(await _context.Periods.Where(x => x.EmployeeId == employeeId).ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Period period)
    {
        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == period.EmployeeId)) return BadRequest("Invalid employee.");
        _context.Periods.Add(period);
        await _context.SaveChangesAsync();
        return Ok(period);
    }

    [HttpDelete("{periodId}")]
    public async Task<IActionResult> Delete(string periodId)
    {
        var period = await _context.Periods.FindAsync(periodId);
        if (period == null) return NotFound();
        _context.Periods.Remove(period);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
