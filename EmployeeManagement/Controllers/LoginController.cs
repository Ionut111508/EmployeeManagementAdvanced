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
        var fallbackRole = account.Username.Contains("admin", StringComparison.OrdinalIgnoreCase) || account.AccountId.Equals("ACC001", StringComparison.OrdinalIgnoreCase) ? "Admin" : isManager ? "Manager" : "Employee";
        var role = await ReadAccountRoleAsync(account.AccountId, fallbackRole);
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

    private async Task<string> ReadAccountRoleAsync(string accountId, string fallbackRole)
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Role FROM Account WHERE AccountId = @accountId";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@accountId";
            parameter.Value = accountId;
            command.Parameters.Add(parameter);
            if (connection.State != System.Data.ConnectionState.Open) await connection.OpenAsync();
            var value = await command.ExecuteScalarAsync();
            return value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()) ? fallbackRole : value.ToString()!;
        }
        catch
        {
            return fallbackRole;
        }
    }
}
