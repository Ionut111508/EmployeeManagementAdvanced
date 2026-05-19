namespace EmployeeManagement.DTOs
{
    public class DepartmentDto
    {
        public string DepartmentId { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    }

    public class DepartmentCreateDto
    {
        public string DepartmentId { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    }

    public class DepartmentUpdateDto
    {
        public string DepartmentName { get; set; } = null!;
    }
}
