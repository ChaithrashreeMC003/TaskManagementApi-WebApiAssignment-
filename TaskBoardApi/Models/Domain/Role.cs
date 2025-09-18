namespace TaskBoardApi.Models.Domain
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!; // Admin, Manager, Developer

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
