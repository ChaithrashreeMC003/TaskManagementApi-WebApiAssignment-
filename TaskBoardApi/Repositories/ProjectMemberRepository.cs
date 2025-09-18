using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Exceptions;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly AppDbContext _context;

        public ProjectMemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectMember>> GetByProjectIdAsync(Guid projectId)
        {
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == projectId);
            if (!projectExists)
                throw new ProjectNotFoundException(projectId);

            return await _context.ProjectMembers
                .Where(m => m.ProjectId == projectId)
                .Include(m => m.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetAsync(Guid projectId, Guid userId)
        {
            return await _context.ProjectMembers
                .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.UserId == userId);
        }

        public async Task<ProjectMember> AddAsync(ProjectMember member, Guid actingUserId, bool isManager)
        {
            var project = await _context.Projects.FindAsync(member.ProjectId);
            if (project == null)
                throw new ProjectNotFoundException(member.ProjectId);

            if (isManager)
            {
                var createdBy = _context.Entry(project).Property("CreatedBy").CurrentValue?.ToString();
                if (createdBy != actingUserId.ToString())
                    throw new UnauthorizedProjectAccessException(member.ProjectId);
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == member.UserId);
            if (!userExists)
                throw new UserNotFoundException(member.UserId);

            var existing = await GetAsync(member.ProjectId, member.UserId);
            if (existing != null)
                throw new MemberAlreadyExistsException(member.ProjectId, member.UserId);

            _context.ProjectMembers.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<ProjectMember> ReplaceMemberAsync(
            Guid projectId,
            Guid oldUserId,
            Guid newUserId,
            Guid actingUserId,
            bool isManager)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new ProjectNotFoundException(projectId);

            if (isManager)
            {
                var createdBy = _context.Entry(project).Property("CreatedBy").CurrentValue?.ToString();
                if (createdBy != actingUserId.ToString())
                    throw new UnauthorizedProjectAccessException(projectId);
            }

            var oldMember = await GetAsync(projectId, oldUserId);
            if (oldMember == null)
                throw new MemberNotFoundException(projectId, oldUserId);

            var newUserExists = await _context.Users.AnyAsync(u => u.Id == newUserId);
            if (!newUserExists)
                throw new UserNotFoundException(newUserId);

            var newMemberExists = await GetAsync(projectId, newUserId);
            if (newMemberExists != null)
                throw new MemberAlreadyExistsException(projectId, newUserId);

            _context.ProjectMembers.Remove(oldMember);

            var newMember = new ProjectMember
            {
                ProjectId = projectId,
                UserId = newUserId
            };

            _context.ProjectMembers.Add(newMember);
            await _context.SaveChangesAsync();

            return newMember;
        }

        public async Task RemoveAsync(Guid projectId, Guid userId, Guid actingUserId, bool isManager)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new ProjectNotFoundException(projectId);

            if (isManager)
            {
                var createdBy = _context.Entry(project).Property("CreatedBy").CurrentValue?.ToString();
                if (createdBy != actingUserId.ToString())
                    throw new UnauthorizedProjectAccessException(projectId);
            }

            var member = await GetAsync(projectId, userId);
            if (member == null)
                throw new MemberNotFoundException(projectId, userId);

            _context.ProjectMembers.Remove(member);
            await _context.SaveChangesAsync();
        }

      
    }
}
