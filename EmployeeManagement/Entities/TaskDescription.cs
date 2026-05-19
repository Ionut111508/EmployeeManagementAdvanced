namespace EmployeeManagement.Entities
{
    public class TaskDescription
    {
        public string TaskDescriptionId { get; set; } = null!;
        public string DescriptionText { get; set; } = null!;
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
