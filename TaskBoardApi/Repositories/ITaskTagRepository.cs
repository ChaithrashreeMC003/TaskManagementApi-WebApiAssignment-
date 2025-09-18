using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface ITaskTagRepository
    {
        Task<IEnumerable<Tag>> GetTagsForTaskAsync(Guid taskId);
        Task<bool> AddTagToTaskAsync(Guid taskId, Guid tagId);
        Task<bool> RemoveTagFromTaskAsync(Guid taskId, Guid tagId);
    }
}

