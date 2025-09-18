using TaskBoardApi.Data;

namespace TaskBoardApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context,
                          ITaskItemRepository taskItems,
                          ITaskAssignmentRepository taskAssignments)
        {
            _context = context;
            TaskItems = taskItems;
            TaskAssignments = taskAssignments;
        }

        public ITaskItemRepository TaskItems { get; }
        public ITaskAssignmentRepository TaskAssignments { get; }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}

