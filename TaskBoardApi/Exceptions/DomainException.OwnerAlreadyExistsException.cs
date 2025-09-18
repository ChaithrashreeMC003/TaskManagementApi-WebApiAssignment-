namespace TaskBoardApi.Exceptions
{
    public abstract partial class DomainException
    {
        public class OwnerAlreadyExistsException : DomainException
        {
            public OwnerAlreadyExistsException(Guid userId)
                : base("OWNER_ALREADY_EXISTS", $"User with Id {userId} is already the owner of another project.")
            {
            }
        }
    }
}
