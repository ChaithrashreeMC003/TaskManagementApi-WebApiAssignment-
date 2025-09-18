using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Exceptions;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> AddAsync(Project project)
        {
            // ✅ Check if the OwnerId exists in Users table
            var ownerExists = await _context.Users.AnyAsync(u => u.Id == project.OwnerId);
            if (!ownerExists)
            {
                throw new UserNotFoundException(project.OwnerId);
            }

            // ✅ Ensure project name is unique
            var nameExists = await _context.Projects
                .AnyAsync(p => p.Name.ToLower() == project.Name.ToLower());

            if (nameExists)
            {
                throw new ProjectNameAlreadyExistsException(project.Name);
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }


        public async Task<Project?> UpdateAsync(Project project)
        {
            var existing = await _context.Projects.FindAsync(project.Id);
            if (existing == null)
            {
                throw new ProjectNotFoundException(project.Id);
            }

            // Ensure new owner exists
            var ownerExists = await _context.Users.AnyAsync(u => u.Id == project.OwnerId);
            if (!ownerExists)
            {
                throw new UserNotFoundException(project.OwnerId);
            }

            // Apply updates
            _context.Entry(existing).CurrentValues.SetValues(project);

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Project?> DeleteAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return null;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<bool> OwnerExistsAsync(Guid ownerId)
        {
            return await _context.Users.AnyAsync(u => u.Id == ownerId);
        }
        public string GetShadowProperty(Project project, string propertyName)
        {
            var entry = _context.Entry(project);

            if (!entry.Metadata.GetProperties().Any(p => p.Name == propertyName))
                throw new ArgumentException($"Shadow property '{propertyName}' does not exist on Project.");

            var value = entry.Property(propertyName).CurrentValue;
            return value?.ToString() ?? string.Empty;
        }
    }
}
