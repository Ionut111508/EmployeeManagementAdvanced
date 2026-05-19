namespace EmployeeManagement.DTOs
{
    public class TaskDescriptionDto
    {
        public string DescriptionId { get; set; } = null!;
        public string TaskDescriptionText { get; set; } = null!;
    }

    public class TaskDescriptionCreateDto
    {
        public string DescriptionId { get; set; } = null!;
        public string TaskDescriptionText { get; set; } = null!;
    }

    public class TaskDescriptionUpdateDto
    {
        public string TaskDescriptionText { get; set; } = null!;
    }
}
