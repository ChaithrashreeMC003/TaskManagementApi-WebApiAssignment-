using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;


namespace TaskBoardManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // All endpoints require auth
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectRepository projectRepo, IMapper mapper)
        {
            _projectRepo = projectRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            var projects = await _projectRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            if (project == null) return NotFound();

            return Ok(_mapper.Map<ProjectDto>(project));
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] // Only Admins/Managers can create
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto dto)
        {
            var project = _mapper.Map<Project>(dto);
            project.Id = Guid.NewGuid();

            var created = await _projectRepo.AddAsync(project);
           
            var result = _mapper.Map<ProjectDto>(created);

            return CreatedAtAction(nameof(GetProject), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectDto dto)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            if (project == null) return NotFound();

            _mapper.Map(dto, project);
            await _projectRepo.UpdateAsync(project);
            var responsedto = _mapper.Map<ProjectDto>(project);
            return Ok(responsedto);
        }


        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchProject(Guid id, PatchProjectDto dto)
        {
            var project = await _projectRepo.GetByIdAsync(id);
            if (project == null) return NotFound();

            // Apply only provided fields
            if (dto.Name is not null) project.Name = dto.Name;
            if (dto.Description is not null) project.Description = dto.Description;
            if (dto.OwnerId.HasValue)
            {
                // validate new owner
                var ownerExists = await _projectRepo.OwnerExistsAsync(dto.OwnerId.Value);
                if (!ownerExists)
                {
                    return BadRequest($"Owner with Id {dto.OwnerId.Value} does not exist.");
                }
                project.OwnerId = dto.OwnerId.Value;
            }
            await _projectRepo.UpdateAsync(project);
            return Ok(_mapper.Map<ProjectDto>(project));
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")] 
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var deleted = await _projectRepo.DeleteAsync(id);
            if (deleted == null) return NotFound();

            return Ok(_mapper.Map<ProjectDto>(deleted));
        }
    }
}