using System.ComponentModel.DataAnnotations;
using TaskBoardApi.Enums;
using TaskStatus = TaskBoardApi.Enums.TaskStatus;

namespace TaskBoardApi.Models.DTOs
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public Guid? AssignedToUserId { get; set; }


        [Required]
        public TaskStatus Status { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }
    }
}
