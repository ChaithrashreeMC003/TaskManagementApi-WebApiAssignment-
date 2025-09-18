namespace TaskBoardApi.Models.DTOs
{
    public class UpdateUserDto
    {
        public string Email { get; set; } = default!;
        public int RoleId { get; set; }
    }
}
