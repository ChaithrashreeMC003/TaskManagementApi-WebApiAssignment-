using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(User user, string roleName);
    }
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Tag>> GetAllAsync() =>
            await _context.Tags.AsNoTracking().ToListAsync();

        public async Task<Tag?> GetByIdAsync(Guid id) =>
            await _context.Tags.FindAsync(id);

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            if (!_context.Tags.Any(t => t.Id == tag.Id)) return null;

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

