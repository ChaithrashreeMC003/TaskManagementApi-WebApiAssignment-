namespace TaskBoardApi.Models.Domain
{
    public class TaskAssignment
    {
        public Guid Id { get; set; }
        public Guid TaskItemId { get; set; }
        public Guid AssignedToUserId { get; set; }
        public Guid AssignedByUserId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string? Comment { get; set; }

        // Navigation
        public TaskItem TaskItem { get; set; } = default!;
        public User AssignedToUser { get; set; } = default!;
        public User AssignedByUser { get; set; } = default!;
    }
}
