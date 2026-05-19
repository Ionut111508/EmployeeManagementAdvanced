namespace EmployeeManagement.DTOs
{
    public class EmployeeDto
    {
        public string EmployeeId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public string? WorkNormId { get; set; }
        public AccountDto? Account { get; set; }
        public WorkNormDto? WorkNorm { get; set; }
    }

    public class EmployeeCreateDto
    {
        public string EmployeeId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public string? WorkNormId { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public string? WorkNormId { get; set; }
    }
}
