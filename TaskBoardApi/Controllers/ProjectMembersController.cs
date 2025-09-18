using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;

namespace TaskBoardApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectMembersController : ControllerBase
    {
        private readonly IProjectMemberRepository _repository;
        private readonly IMapper _mapper;

        public ProjectMembersController(IProjectMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private Guid GetLoggedInUserId() =>
            Guid.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        private bool IsManager() => User.IsInRole("Manager");
        [AllowAnonymous]
        [HttpGet("{projectId}")]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetMembers(Guid projectId)
        {
            var members = await _repository.GetByProjectIdAsync(projectId);
            return Ok(_mapper.Map<IEnumerable<ProjectMemberDto>>(members));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ProjectMemberDto>> AddMember(CreateProjectMemberDto dto)
        {
            var member = _mapper.Map<ProjectMember>(dto);
            var created = await _repository.AddAsync(member, GetLoggedInUserId(), IsManager());

            return CreatedAtAction(nameof(GetMembers),
                new { projectId = created.ProjectId },
                _mapper.Map<ProjectMemberDto>(created));
        }

        [HttpPut] // can also use [HttpPatch]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchMember(PatchProjectMemberDto dto)
        {
            await _repository.ReplaceMemberAsync(dto.ProjectId, dto.UserId, dto.NewUserId, GetLoggedInUserId(), IsManager());
            return NoContent();
        }

        [HttpDelete("{projectId}/{userId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> RemoveMember(Guid projectId, Guid userId)
        {
            await _repository.RemoveAsync(projectId, userId, GetLoggedInUserId(), IsManager());
            return NoContent();
        }
    }
}
