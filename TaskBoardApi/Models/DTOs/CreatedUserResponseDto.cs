namespace TaskBoardApi.Models.DTOs
{
    public class CreatedUserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public int RoleId { get; set; }
    }
}
