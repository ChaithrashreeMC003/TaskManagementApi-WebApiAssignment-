namespace TaskBoardApi.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid userId)
            : base("USER_NOT_FOUND", $"User with Id {userId} does not exist.") { }
    }
}
