namespace EmployeeManagement.Entities
{
    public class ProjectManager
    {
        public string EmployeeId { get; set; } = null!;
        public string ProjectId { get; set; } = null!;
        public DateTime? AssignedDate { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }
        public Project? Project { get; set; }
    }
}
