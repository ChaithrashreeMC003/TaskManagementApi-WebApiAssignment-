using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;

namespace TaskBoardApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _profileRepo;
        private readonly IMapper _mapper;

        public UserProfileController(IUserProfileRepository profileRepo, IMapper mapper)
        {
            _profileRepo = profileRepo;
            _mapper = mapper;
        }

        // Centralized method to get logged-in user's ID from claims
        private Guid GetLoggedInUserId()
        {
            // Try "sub" first (used by shadow properties), then NameIdentifier
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                              ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID claim not found in token.");

            return Guid.Parse(userIdClaim);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetLoggedInUserId();

            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null) return NotFound();

            var dto = _mapper.Map<UserProfileDto>(profile);
            return Ok(dto);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserProfileDto dto)
        {
            var userId = GetLoggedInUserId();

            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null) return NotFound();

            // Map DTO fields to profile
            profile.FullName = dto.FullName;
            profile.Phone = dto.Phone;
            profile.Address = dto.Address;

            var updated = await _profileRepo.UpdateAsync(profile);
            var result = _mapper.Map<UpdateUserProfileDto>(updated);
            return Ok(result);
        }

        [HttpPatch("me")]
        public async Task<IActionResult> PatchMyProfile([FromBody] PatchUserProfileDto dto)
        {
            var userId = GetLoggedInUserId();

            var profile = await _profileRepo.GetByUserIdAsync(userId);
            if (profile == null) return NotFound();

            // Apply only provided fields
            if (!string.IsNullOrEmpty(dto.FullName))
                profile.FullName = dto.FullName;

            if (!string.IsNullOrEmpty(dto.Phone))
                profile.Phone = dto.Phone;

            if (!string.IsNullOrEmpty(dto.Address))
                profile.Address = dto.Address;

            var updated = await _profileRepo.UpdateAsync(profile);
            var result = _mapper.Map<UserProfileDto>(updated);
            return Ok(result);
        }
    }
}
