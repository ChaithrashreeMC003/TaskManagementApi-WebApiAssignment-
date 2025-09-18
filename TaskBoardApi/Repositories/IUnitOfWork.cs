namespace TaskBoardApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskItemRepository TaskItems { get; }
        ITaskAssignmentRepository TaskAssignments { get; }
        Task<int> CompleteAsync();
    }
}

