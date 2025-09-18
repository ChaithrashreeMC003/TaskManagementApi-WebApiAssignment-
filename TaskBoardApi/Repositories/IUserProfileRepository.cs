using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByUserIdAsync(Guid userId);
        Task<UserProfile?> UpdateAsync(UserProfile profile);
    }
}
