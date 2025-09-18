using System.ComponentModel.DataAnnotations;

namespace TaskBoardApi.Models.DTOs
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Project Name is required")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "OwnerId is required")]
        public Guid OwnerId { get; set; }
    }
}
