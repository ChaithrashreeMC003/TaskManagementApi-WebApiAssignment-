using System.Data;

namespace TaskBoardApi.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public int RoleId { get; set; }

        // Navigation
        public Role Role { get; set; } = default!;
        public UserProfile? Profile { get; set; }
        public ICollection<Project> Projects { get; set; } = new List<Project>(); // Owned projects
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();  // Assigned tasks
        public ICollection<TaskAssignment> Assignments { get; set; } = new List<TaskAssignment>(); // History
    }
    
}
