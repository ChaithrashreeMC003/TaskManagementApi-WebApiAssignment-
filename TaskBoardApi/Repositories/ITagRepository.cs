using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag?> GetByIdAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<bool> DeleteAsync(Guid id);
    }

}

