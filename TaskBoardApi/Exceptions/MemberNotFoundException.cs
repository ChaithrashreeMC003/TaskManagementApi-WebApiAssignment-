public class MemberNotFoundException : Exception
{
    public string ErrorCode => "MEMBER_NOT_FOUND";
    public MemberNotFoundException(Guid projectId, Guid userId)
        : base($"User {userId} is not a member of project {projectId}.") { }
}