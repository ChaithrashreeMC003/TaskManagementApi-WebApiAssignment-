namespace TaskBoardApi.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = default!;
    }
}
