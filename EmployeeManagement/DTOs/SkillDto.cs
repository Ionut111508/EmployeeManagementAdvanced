namespace EmployeeManagement.DTOs
{
    public class SkillDto
    {
        public string SkillId { get; set; } = null!;
        public string SkillName { get; set; } = null!;
        public string? SkillLevel { get; set; }
    }

    public class SkillCreateDto
    {
        public string SkillId { get; set; } = null!;
        public string SkillName { get; set; } = null!;
        public string? SkillLevel { get; set; }
    }

    public class SkillUpdateDto
    {
        public string SkillName { get; set; } = null!;
        public string? SkillLevel { get; set; }
    }
}
