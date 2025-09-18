namespace TaskBoardApi.Exceptions
{
    public class ProjectNotFoundException : DomainException
    {
        public ProjectNotFoundException(Guid projectId)
            : base("PROJECT_NOT_FOUND", $"Project with Id {projectId} does not exist.") { }
    }
}
