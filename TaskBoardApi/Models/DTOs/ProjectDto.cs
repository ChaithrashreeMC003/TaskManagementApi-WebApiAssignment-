namespace TaskBoardApi.Models.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public Guid OwnerId { get; set; }
    }
}
