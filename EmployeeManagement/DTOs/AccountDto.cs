namespace EmployeeManagement.DTOs
{
    public class AccountDto
    {
        public string AccountId { get; set; } = null!;
        public string Username { get; set; } = null!;
    }

    public class AccountCreateDto
    {
        public string AccountId { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AccountUpdateDto
    {
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
    }
}
