using System.ComponentModel.DataAnnotations;
using TaskBoardApi.Enums;
using TaskStatus = TaskBoardApi.Enums.TaskStatus;

namespace TaskBoardApi.Models.DTOs
{
    public class PatchTaskItemDto
    {
        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public Guid? AssignedToUserId { get; set; }

        [EnumDataType(typeof(TaskStatus))]
        public TaskStatus? Status { get; set; }

        [EnumDataType(typeof(TaskPriority))]
        public TaskPriority? Priority { get; set; }

    }
}
