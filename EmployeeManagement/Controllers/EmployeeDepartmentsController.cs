using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeDepartmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeDepartmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _context.EmployeeDepartments
            .Include(x => x.Employee)
            .Include(x => x.Department)
            .ToListAsync();
        return Ok(result);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId)
    {
        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == employeeId))
            return NotFound("Employee was not found.");

        var result = await _context.EmployeeDepartments
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Department)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(string departmentId)
    {
        if (!await _context.Departments.AnyAsync(x => x.DepartmentId == departmentId))
            return NotFound("Department was not found.");

        var result = await _context.EmployeeDepartments
            .Where(x => x.DepartmentId == departmentId)
            .Include(x => x.Employee)
            .Include(x => x.Department)
            .ToListAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeDepartmentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.EmployeeId) || string.IsNullOrWhiteSpace(request.DepartmentId))
            return BadRequest("EmployeeId and DepartmentId are required.");

        if (request.StartDate == default)
            return BadRequest("StartDate is required.");

        if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
            return BadRequest("EndDate cannot be before StartDate.");

        if (!await _context.Employees.AnyAsync(x => x.EmployeeId == request.EmployeeId))
            return BadRequest("Selected employee does not exist.");

        if (!await _context.Departments.AnyAsync(x => x.DepartmentId == request.DepartmentId))
            return BadRequest("Selected department does not exist.");

        var exists = await _context.EmployeeDepartments.FindAsync(request.EmployeeId, request.DepartmentId);
        if (exists != null)
            return BadRequest("Employee is already assigned to this department.");

        var item = new EmployeeDepartment
        {
            EmployeeId = request.EmployeeId,
            DepartmentId = request.DepartmentId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.EmployeeDepartments.Add(item);
        await _context.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{employeeId}/{departmentId}")]
    public async Task<IActionResult> Delete(string employeeId, string departmentId)
    {
        var item = await _context.EmployeeDepartments.FindAsync(employeeId, departmentId);
        if (item == null) return NotFound("Employee department assignment was not found.");

        _context.EmployeeDepartments.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
