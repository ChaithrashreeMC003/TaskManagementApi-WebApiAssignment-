using TaskBoardApi.Exceptions;

public class OwnerAlreadyAssignedException : DomainException
{
    public OwnerAlreadyAssignedException(Guid ownerId, Guid projectId)
        : base("OWNER_ALREADY_ASSIGNED", $"User with Id {ownerId} is already the owner of project {projectId}.")
    {
    }
}