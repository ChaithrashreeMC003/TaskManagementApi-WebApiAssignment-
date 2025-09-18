using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Services;

namespace TaskBoardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TasksController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
        }

        //  helper method for extracting logged-in user id
        private Guid GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst("UserId") ?? User.FindFirst("userId");

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId claim missing in token");

            return Guid.Parse(userIdClaim.Value);
        }

        //  Create
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        //[Authorize(Policy = "ProjectOwnerOrManager")]
        public async Task<ActionResult<TaskItemDto>> CreateTask([FromBody] CreateTaskItemDto dto)
        {
            var createdByUserId = GetLoggedInUserId();

            var task = await _taskService.CreateTaskAsync(dto, createdByUserId);
            if (task == null)
                return BadRequest($"Project {dto.ProjectId} or Assigned User {dto.AssignedToUserId} does not exist.");

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, _mapper.Map<TaskItemDto>(task));
        }

        //  Get All
        [HttpGet]

        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasks()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(tasks));
        }

        //  Get by Id
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TaskItemDto>> GetTask(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();

            return Ok(_mapper.Map<TaskItemDto>(task));
        }

        // Update (PUT)
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskItemDto dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(_mapper.Map<TaskItemDto>(updated));
        }

        //  Patch (Partial Update)
        [HttpPatch("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchTask(Guid id, [FromBody] PatchTaskItemDto dto)
        {
            var updated = await _taskService.PatchAsync(id, dto);
            if (updated == null) return NotFound();

            return Ok(_mapper.Map<TaskItemDto>(updated));
        }

        //  Delete
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var deleted = await _taskService.DeleteAsync(id);
            if (deleted == null) return NotFound();

            return Ok(_mapper.Map<TaskItemDto>(deleted));
        }
    }
}

