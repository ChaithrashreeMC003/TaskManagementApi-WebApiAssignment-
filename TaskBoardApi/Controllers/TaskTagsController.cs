using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;

namespace TaskBoardApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTagsController : ControllerBase
    {
        private readonly ITaskTagRepository _taskTagRepo;
        private readonly IMapper _mapper;

        public TaskTagsController(ITaskTagRepository taskTagRepo, IMapper mapper)
        {
            _taskTagRepo = taskTagRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTagsForTask(Guid taskId)
        {
            var tags = await _taskTagRepo.GetTagsForTaskAsync(taskId);
            return Ok(_mapper.Map<IEnumerable<TagDto>>(tags));
        }

        [HttpPost]
        public async Task<IActionResult> AddTagToTask(Guid taskId, AddTagToTaskDto dto)
        {
            var success = await _taskTagRepo.AddTagToTaskAsync(taskId, dto.TagId);
            if (!success) return BadRequest("Tag already exists for this task or invalid.");
            return Ok(new { message = "Tag added to task successfully" });
        }

        [HttpDelete("{tagId:guid}")]
        public async Task<IActionResult> RemoveTagFromTask(Guid taskId, Guid tagId)
        {
            var success = await _taskTagRepo.RemoveTagFromTaskAsync(taskId, tagId);
            if (!success) return NotFound("Tag not found for this task.");
            return Ok(new { message = "Tag removed from task successfully" });
        }
    }
}
