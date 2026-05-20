using EmployeeManagement.Data;
using EmployeeManagement.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly AppDbContext _context;

    public LoginController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Username == request.Username && a.Password == request.Password);
        if (account == null) return Unauthorized("Invalid username or password.");

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.AccountId == account.AccountId);
        var isManager = employee != null && await _context.ProjectManagers.AnyAsync(pm => pm.EmployeeId == employee.EmployeeId);
        var role = account.Username.Contains("admin", StringComparison.OrdinalIgnoreCase) || account.AccountId.Equals("ACC001", StringComparison.OrdinalIgnoreCase) ? "Admin" : isManager ? "Manager" : "Employee";
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return Ok(new LoginResponse
        {
            Token = $"demo.{account.AccountId}.{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
            Username = account.Username,
            Role = role,
            FullName = employee == null ? account.Username : employee.FirstName + " " + employee.LastName,
            EmployeeId = employee?.EmployeeId,
            ExpiresAt = expiresAt
        });
    }
}
