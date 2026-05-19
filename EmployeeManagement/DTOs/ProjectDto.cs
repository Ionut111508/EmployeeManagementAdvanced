namespace EmployeeManagement.DTOs
{
    public class ProjectDto
    {
        public string ProjectId { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
    }

    public class ProjectCreateDto
    {
        public string ProjectId { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
    }

    public class ProjectUpdateDto
    {
        public string ProjectName { get; set; } = null!;
    }
}
