using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AllocationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAllocationService _service;

    public AllocationsController(AppDbContext context, IAllocationService service)
    {
        _context = context;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("employee/{employeeId}")]
    public async Task<IActionResult> GetByEmployee(string employeeId) => Ok(await _service.GetByEmployeeAsync(employeeId));

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProject(string projectId) => Ok(await _service.GetByProjectAsync(projectId));

    [HttpGet("task/{projectId}/{taskId}")]
    public async Task<IActionResult> GetByTask(string projectId, string taskId) => Ok(await _service.GetByTaskAsync(projectId, taskId));

    [HttpPost]
    public async Task<IActionResult> Create(CreateAllocationRequest request)
    {
        var result = await _service.CreateAllocationAsync(request);
        return result.Success ? Ok(result.Allocation) : BadRequest(result.Error);
    }

    [HttpDelete("{employeeId}/{projectId}/{taskId}")]
    public async Task<IActionResult> Delete(string employeeId, string projectId, string taskId)
    {
        var allocation = await _context.Allocations.FindAsync(employeeId, projectId, taskId);
        if (allocation == null) return NotFound();
        _context.Allocations.Remove(allocation);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
