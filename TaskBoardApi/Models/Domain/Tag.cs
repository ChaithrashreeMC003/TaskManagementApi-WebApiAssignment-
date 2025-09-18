namespace TaskBoardApi.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? ColorHex { get; set; }

        // Navigation
        public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
    }
}
