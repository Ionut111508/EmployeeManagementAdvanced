using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Entities;
using EmployeeManagement.DTOs;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            var employees = await _context.Employees
                .Include(e => e.Account)
                .Include(e => e.WorkNorm)
                .ToListAsync();

            var dtos = employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                LastName = e.LastName,
                FirstName = e.FirstName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                AccountId = e.AccountId,
                WorkNormId = e.WorkNormId,
                Account = e.Account != null ? new AccountDto { AccountId = e.Account.AccountId, Username = e.Account.Username } : null,
                WorkNorm = e.WorkNorm != null ? new WorkNormDto { WorkNormId = e.WorkNorm.WorkNormId, WorkNormName = e.WorkNorm.WorkNormName, WorkHours = e.WorkNorm.WorkHours } : null
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(string id)
        {
            var employee = await _context.Employees
                .Include(e => e.Account)
                .Include(e => e.WorkNorm)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            var dto = new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                AccountId = employee.AccountId,
                WorkNormId = employee.WorkNormId,
                Account = employee.Account != null ? new AccountDto { AccountId = employee.Account.AccountId, Username = employee.Account.Username } : null,
                WorkNorm = employee.WorkNorm != null ? new WorkNormDto { WorkNormId = employee.WorkNorm.WorkNormId, WorkNormName = employee.WorkNorm.WorkNormName, WorkHours = employee.WorkNorm.WorkHours } : null
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(EmployeeCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.EmployeeId) || string.IsNullOrWhiteSpace(dto.LastName) || string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest("EmployeeId, LastName, and FirstName are required");

            var employee = new Employee
            {
                EmployeeId = dto.EmployeeId,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                AccountId = dto.AccountId,
                WorkNormId = dto.WorkNormId
            };

            _context.Employees.Add(employee);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await _context.Employees.AnyAsync(e => e.EmployeeId == dto.EmployeeId))
                    return BadRequest("Employee with this ID already exists");
                if (dto.AccountId != null && await _context.Employees.AnyAsync(e => e.AccountId == dto.AccountId))
                    return BadRequest("This Account is already assigned to another employee");
                throw;
            }

            await _context.Entry(employee).Reference(e => e.Account).LoadAsync();
            await _context.Entry(employee).Reference(e => e.WorkNorm).LoadAsync();

            var resultDto = new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                AccountId = employee.AccountId,
                WorkNormId = employee.WorkNormId,
                Account = employee.Account != null ? new AccountDto { AccountId = employee.Account.AccountId, Username = employee.Account.Username } : null,
                WorkNorm = employee.WorkNorm != null ? new WorkNormDto { WorkNormId = employee.WorkNorm.WorkNormId, WorkNormName = employee.WorkNorm.WorkNormName, WorkHours = employee.WorkNorm.WorkHours } : null
            };

            return CreatedAtAction(nameof(GetById), new { id = employee.EmployeeId }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, EmployeeUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LastName) || string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest("LastName and FirstName are required");

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            employee.LastName = dto.LastName;
            employee.FirstName = dto.FirstName;
            employee.Email = dto.Email;
            employee.PhoneNumber = dto.PhoneNumber;

            if (dto.AccountId != employee.AccountId)
            {
                if (dto.AccountId != null && await _context.Employees.AnyAsync(e => e.AccountId == dto.AccountId && e.EmployeeId != id))
                    return BadRequest("This Account is already assigned to another employee");
                employee.AccountId = dto.AccountId;
            }

            employee.WorkNormId = dto.WorkNormId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
