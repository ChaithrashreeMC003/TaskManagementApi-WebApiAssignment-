using System.ComponentModel.DataAnnotations;

namespace TaskBoardApi.Models.DTOs
{
    public class CreateProjectMemberDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
