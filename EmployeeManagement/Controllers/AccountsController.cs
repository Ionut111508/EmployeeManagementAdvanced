using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Data;
using EmployeeManagement.Entities;
using EmployeeManagement.DTOs;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var accounts = await _context.Accounts.ToListAsync();
            var dtos = accounts.Select(a => new AccountDto { AccountId = a.AccountId, Username = a.Username }).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetById(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            return Ok(new AccountDto { AccountId = account.AccountId, Username = account.Username });
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(AccountCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AccountId) || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("AccountId, Username, and Password are required");

            var account = new Account
            {
                AccountId = dto.AccountId,
                Username = dto.Username,
                Password = dto.Password
            };

            _context.Accounts.Add(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (await _context.Accounts.AnyAsync(a => a.AccountId == dto.AccountId))
                    return BadRequest("Account with this ID already exists");
                throw;
            }

            var resultDto = new AccountDto { AccountId = account.AccountId, Username = account.Username };
            return CreatedAtAction(nameof(GetById), new { id = account.AccountId }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, AccountUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest("Username is required");

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            account.Username = dto.Username;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                account.Password = dto.Password;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
