using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
        }

        public void Update(TaskItem task)
        {
            _context.TaskItems.Update(task);
        }

        public void Delete(TaskItem task)
        {
            _context.TaskItems.Remove(task);
        }
    }
}

