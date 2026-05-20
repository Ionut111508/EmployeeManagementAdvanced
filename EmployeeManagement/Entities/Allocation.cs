using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Entities
{
    public class Allocation
    {
        public string EmployeeId { get; set; } = null!;
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;

        [Column("AllocationStartDate")]
        public DateTime AllocationStartDate { get; set; }

        [Column("AllocationEndDate")]
        public DateTime? AllocationEndDate { get; set; }

        [Column("HoursPerDay")]
        public decimal AllocatedHours { get; set; }

        public Employee? Employee { get; set; }
        public Project? Project { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
