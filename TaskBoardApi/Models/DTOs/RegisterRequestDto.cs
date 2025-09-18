namespace TaskBoardApi.Models.DTOs
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int RoleId { get; set; } // 1=Admin, 2=Manager, 3=Developer
    }
}
