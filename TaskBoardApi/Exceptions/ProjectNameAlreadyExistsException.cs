using TaskBoardApi.Exceptions;

public class ProjectNameAlreadyExistsException : DomainException
{
    public ProjectNameAlreadyExistsException(string name)
        : base("PROJECT_NAME_ALREADY_EXISTS", $"A project with name '{name}' already exists.")
    {
    }
}
