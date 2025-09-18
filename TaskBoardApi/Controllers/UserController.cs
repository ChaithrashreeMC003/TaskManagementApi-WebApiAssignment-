using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApi.Data;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;

namespace TaskBoardApi.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require JWT token for all endpoints
    public class UsersController : ControllerBase
    {
        
        private readonly IMapper mapper;
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo, IMapper mapper)
        {
          
            _userRepo = userRepo;
            this.mapper = mapper;
            
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers(
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 10,
         [FromQuery] string? sortBy = null,
         [FromQuery] string sortOrder = "asc",
         [FromQuery] string? search = null)
        {
            var (users, totalItems,error) = await _userRepo.GetAllAsync(page, pageSize, sortBy, sortOrder, search);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var usersDto = mapper.Map<List<UserDto>>(users);

            return Ok(new
            {
                page,
                pageSize,
                totalPages,
                totalItems,
                items = usersDto,
                error
            });
        }



        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();

            var dto = mapper.Map<UserDto>(user);
            return Ok(dto);
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            var user = mapper.Map<User>(dto);

            var createdUser = await _userRepo.AddAsync(user);

            if (createdUser == null)
            {
                return BadRequest($"Invalid RoleId {dto.RoleId}. Allowed values are Admin=1, Manager=2, Developer=3.");
            }

            var result = mapper.Map<CreatedUserResponseDto>(createdUser);

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto dto)
        {
            // Build a User object from DTO
            var userToUpdate = new User
            {
                Id = id,
                Email = dto.Email,
                RoleId = dto.RoleId
            };

            // Call repository
            var result = await _userRepo.UpdateAsync(userToUpdate);

            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
            var resultdto = mapper.Map<CreatedUserResponseDto>(result.UpdatedUser);
            return Ok(resultdto); // return updated user as confirmation
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deletedUser = await _userRepo.DeleteAsync(id);

            if (deletedUser == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<CreatedUserResponseDto>(deletedUser);

            return Ok(dto); // return deleted data
        }

        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchUser(Guid id, PatchUserDto dto)
        {
            if (dto == null)
                return BadRequest("Patch data cannot be null.");

            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            // Apply only provided fields
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            // Only update RoleId if it's provided (not zero)
            if (dto.RoleId != 0)
            {
                user.RoleId = dto.RoleId;
            }

            // Do NOT allow patching sensitive fields like PasswordHash or Id

            await _userRepo.UpdateAsync(user);

            return Ok(mapper.Map<UserDto>(user));
        }


    }
}
