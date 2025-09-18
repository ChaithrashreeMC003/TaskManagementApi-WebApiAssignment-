using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<Project> AddAsync(Project project);
        Task<Project?> UpdateAsync(Project project);
        Task<Project?> DeleteAsync(Guid id);
        Task<bool> OwnerExistsAsync(Guid ownerId);
        string GetShadowProperty(Project project, string v);
    }
}
