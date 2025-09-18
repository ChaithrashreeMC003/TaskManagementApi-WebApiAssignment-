using TaskBoardApi.Exceptions;

public class OwnerAlreadyExistsException : DomainException
{
    public OwnerAlreadyExistsException(Guid userId)
        : base("OWNER_ALREADY_EXISTS", $"User with Id {userId} is already the owns the project.")
    {
    }
}
