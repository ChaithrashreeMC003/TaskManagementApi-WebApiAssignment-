namespace TaskBoardApi.Models.Domain
{
    public class ProjectMember
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = default!;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
