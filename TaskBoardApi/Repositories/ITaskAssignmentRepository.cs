using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public interface ITaskAssignmentRepository
    {
        Task AddAsync(TaskAssignment assignment);
    }
   

}

