using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task AddAsync(TaskItem task);
        void Update(TaskItem task);
        void Delete(TaskItem task);
    }
}

