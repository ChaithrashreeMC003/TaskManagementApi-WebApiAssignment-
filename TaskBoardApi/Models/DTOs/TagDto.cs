namespace TaskBoardApi.Models.DTOs
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string ColorHex { get; set; } = "#FFFFFF";
    }
}
