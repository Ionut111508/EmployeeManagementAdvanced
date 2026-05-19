namespace EmployeeManagement.Entities
{
    public class TaskComment
    {
        public string TaskCommentId { get; set; } = null!;
        public string CommentText { get; set; } = null!;
        public DateTime? CommentDate { get; set; }
        public string ProjectId { get; set; } = null!;
        public string TaskId { get; set; } = null!;
        public string? EmployeeId { get; set; }

        public TaskItem? TaskItem { get; set; }
        public Employee? Employee { get; set; }
    }
}
