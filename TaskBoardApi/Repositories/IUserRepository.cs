using System.Threading.Tasks;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface IUserRepository
    {
        Task<(IEnumerable<User> Users, int TotalItems, string? Error)> GetAllAsync(
       int page = 1,
       int pageSize = 10,
       string? sortBy = null,
       string sortOrder = "asc",
       string? search = null);
        Task<User?> GetByIdAsync(Guid id);
        Task<User> AddAsync(User user);
        Task<(bool Success, string? ErrorMessage, User? UpdatedUser)> UpdateAsync(User user);
        Task<User?> DeleteAsync(Guid id);
    }
}
