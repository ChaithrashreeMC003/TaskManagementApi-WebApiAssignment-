using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateJwtToken(User user, string roleName)
        {
            var claims = new List<Claim>
            {
                // ✅ Keep existing auditing claims
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),// subject = user ID
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserId", user.Id.ToString()),                     // explicit UserId for auditing
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),                        // keep original role claim (used in auditing)

                // ✅ Add improvements from second snippet
                new Claim("userId", user.Id.ToString()),                     // lowercase key for easy lookup
                new Claim(ClaimTypes.Email, user.Email),                     // standard email claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique token ID
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64) // issued-at timestamp
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // token validity
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
