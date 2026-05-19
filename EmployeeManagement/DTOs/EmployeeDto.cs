namespace EmployeeManagement.DTOs
{
    public class EmployeeDto
    {
        public string EmployeeId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string WorkNormId { get; set; } = null!;
        public AccountDto? Account { get; set; }
        public WorkNormDto? WorkNorm { get; set; }
    }

    public class EmployeeCreateDto
    {
        public string EmployeeId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string WorkNormId { get; set; } = null!;
    }

    public class EmployeeUpdateDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string WorkNormId { get; set; } = null!;
    }
}
