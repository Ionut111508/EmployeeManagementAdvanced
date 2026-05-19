namespace EmployeeManagement.Entities
{
    public class Skill
    {
        public string SkillId { get; set; } = null!;
        public string SkillName { get; set; } = null!;
        public string? SkillLevel { get; set; }

        public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
    }
}
