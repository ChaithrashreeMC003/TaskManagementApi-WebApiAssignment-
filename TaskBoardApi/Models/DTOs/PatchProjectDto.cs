namespace TaskBoardApi.Models.DTOs
{
    public class PatchProjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? OwnerId { get; set; } // optional
    }
}
