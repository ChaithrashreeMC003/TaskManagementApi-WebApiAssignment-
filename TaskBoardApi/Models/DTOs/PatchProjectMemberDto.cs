using System.ComponentModel.DataAnnotations;

namespace TaskBoardApi.Models.DTOs
{
    public class PatchProjectMemberDto
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid UserId { get; set; }  // current user in project

        [Required]
        public Guid NewUserId { get; set; } // user who will replace the current one
    }
}
