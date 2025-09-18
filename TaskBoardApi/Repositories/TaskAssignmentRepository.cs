using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly AppDbContext _context;
        public TaskAssignmentRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(TaskAssignment assignment)
        {
            await _context.TaskAssignments.AddAsync(assignment);
        }
    }
}

