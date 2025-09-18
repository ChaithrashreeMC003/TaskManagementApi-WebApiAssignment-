using System.ComponentModel.DataAnnotations;
using TaskBoardApi.Enums;
using TaskStatus = TaskBoardApi.Enums.TaskStatus;

namespace TaskBoardApi.Models.DTOs
{
    public class UpdateTaskItemDto
    {
        [Required, StringLength(100)]
        public string Title { get; set; } = default!;

        [StringLength(500)]
        public string? Description { get; set; }


        [Required]
        public Guid ProjectId { get; set; }

        public Guid? AssignedToUserId { get; set; }

        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus Status { get; set; }

        [EnumDataType(typeof(TaskPriority))]
        public TaskPriority Priority { get; set; }
    }
}
