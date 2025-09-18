using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly AppDbContext _context;

        public UserProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
        {
            return await _context.UserProfiles
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<UserProfile?> UpdateAsync(UserProfile profile)
        {
            var existing = await _context.UserProfiles.FindAsync(profile.UserId);
            if (existing == null) return null;

            existing.FullName = profile.FullName;
            existing.Phone = profile.Phone;
            existing.Address = profile.Address;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
