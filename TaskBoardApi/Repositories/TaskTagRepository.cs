using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class TaskTagRepository : ITaskTagRepository
    {
        private readonly AppDbContext _context;
        public TaskTagRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Tag>> GetTagsForTaskAsync(Guid taskId)
        {
            return await _context.TaskTags
                .Where(tt => tt.TaskItemId == taskId)
                .Include(tt => tt.Tag)
                .Select(tt => tt.Tag)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> AddTagToTaskAsync(Guid taskId, Guid tagId)
        {
            var exists = await _context.TaskTags
                .AnyAsync(tt => tt.TaskItemId == taskId && tt.TagId == tagId);
            if (exists) return false;

            _context.TaskTags.Add(new TaskTag { TaskItemId = taskId, TagId = tagId });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTagFromTaskAsync(Guid taskId, Guid tagId)
        {
            var taskTag = await _context.TaskTags
                .FirstOrDefaultAsync(tt => tt.TaskItemId == taskId && tt.TagId == tagId);
            if (taskTag == null) return false;

            _context.TaskTags.Remove(taskTag);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

