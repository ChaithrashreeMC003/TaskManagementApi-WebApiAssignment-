public class MemberAlreadyExistsException : Exception
{
    public string ErrorCode => "MEMBER_ALREADY_EXISTS";
    public MemberAlreadyExistsException(Guid projectId, Guid userId)
        : base($"User {userId} is already a member of project {projectId}.") { }
}
