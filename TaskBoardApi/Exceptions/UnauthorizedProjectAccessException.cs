public class UnauthorizedProjectAccessException : Exception
{
    public string ErrorCode => "UNAUTHORIZED_PROJECT_ACCESS";
    public UnauthorizedProjectAccessException(Guid projectId)
        : base($"You do not have permission to modify project {projectId}.") { }
}