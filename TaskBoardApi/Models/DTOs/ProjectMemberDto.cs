namespace TaskBoardApi.Models.DTOs
{
    public class ProjectMemberDto
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}
