using EmployeeManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("employees")]
    public async Task<ActionResult<IEnumerable<EmployeeRoleDto>>> GetEmployeeRoles()
    {
        var managers = await _context.ProjectManagers
            .GroupBy(pm => pm.EmployeeId)
            .Select(group => new { EmployeeId = group.Key, ProjectsCount = group.Count() })
            .ToDictionaryAsync(x => x.EmployeeId, x => x.ProjectsCount);

        var employees = await _context.Employees
            .Include(e => e.Account)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();

        var result = employees.Select(employee =>
        {
            var username = employee.Account?.Username ?? string.Empty;
            var isAdmin = username.Contains("admin", StringComparison.OrdinalIgnoreCase) ||
                          employee.AccountId.Equals("ACC001", StringComparison.OrdinalIgnoreCase);
            var projectsManaged = managers.TryGetValue(employee.EmployeeId, out var count) ? count : 0;
            var role = isAdmin ? "Admin" : projectsManaged > 0 ? "Manager" : "Employee";

            return new EmployeeRoleDto
            {
                EmployeeId = employee.EmployeeId,
                FullName = $"{employee.LastName} {employee.FirstName}",
                Username = username,
                Role = role,
                ProjectsManaged = projectsManaged,
                Description = role switch
                {
                    "Admin" => "Acces complet la configurare, nomenclatoare si resurse.",
                    "Manager" => "Acces la proiecte, task-uri, alocari si pontajele echipei.",
                    _ => "Acces la propriile task-uri, alocari si pontaj."
                }
            };
        }).ToList();

        return Ok(result);
    }
}

public class EmployeeRoleDto
{
    public string EmployeeId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int ProjectsManaged { get; set; }
    public string Description { get; set; } = null!;
}
