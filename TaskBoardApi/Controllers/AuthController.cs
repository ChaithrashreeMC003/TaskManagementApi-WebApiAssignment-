using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;

namespace TaskBoardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(AppDbContext context, ITokenRepository tokenRepository)
        {
            _context = context;
            _tokenRepository = tokenRepository;
        }

       
        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null) return Unauthorized("Invalid email or password.");

            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordValid) return Unauthorized("Invalid email or password.");

            var token = _tokenRepository.CreateJwtToken(user, user.Role.Name);

            return Ok(new LoginResponseDto { JwtToken = token });
        }
    }
}
