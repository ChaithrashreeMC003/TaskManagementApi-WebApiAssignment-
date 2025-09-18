using TaskBoardApi.Enums;

namespace TaskBoardApi.Models.Domain
{
    public class TaskItem
    {
        //Id, Title, Description, ProjectId, AssignedToUserId, Status, Priority
        //Navigation: Assignments, Tags(optional)

        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        // Navigation
        public Project Project { get; set; } = default!;
        public User? AssignedToUser { get; set; }
        public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>();
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
