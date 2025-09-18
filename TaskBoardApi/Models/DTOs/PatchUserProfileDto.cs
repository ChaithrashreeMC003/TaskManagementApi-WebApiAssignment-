namespace TaskBoardApi.Models.DTOs
{
    public class PatchUserProfileDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
