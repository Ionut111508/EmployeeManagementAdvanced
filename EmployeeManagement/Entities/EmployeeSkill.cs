namespace EmployeeManagement.Entities
{
    public class EmployeeSkill
    {
        public string EmployeeId { get; set; } = null!;
        public string SkillId { get; set; } = null!;
        public DateTime? AcquiredDate { get; set; }

        // Navigation properties
        public Employee? Employee { get; set; }
        public Skill? Skill { get; set; }
    }
}
