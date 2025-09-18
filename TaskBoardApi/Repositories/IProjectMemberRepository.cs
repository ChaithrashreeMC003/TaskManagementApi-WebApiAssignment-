using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface IProjectMemberRepository
    {
        Task<IEnumerable<ProjectMember>> GetByProjectIdAsync(Guid projectId);
        Task<ProjectMember?> GetAsync(Guid projectId, Guid userId);
        Task<ProjectMember> AddAsync(ProjectMember member, Guid actingUserId, bool isManager);
        Task<ProjectMember> ReplaceMemberAsync(
           Guid projectId,
           Guid oldUserId,
           Guid newUserId,
           Guid actingUserId,
           bool isManager);
        Task RemoveAsync(Guid projectId, Guid userId, Guid actingUserId, bool isManager);
    }
   

}

