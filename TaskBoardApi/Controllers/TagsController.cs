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
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public TagsController(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET all tags
        [HttpGet]
        [Authorize(Roles = "Admin,Manager,Developer")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            var tags = await _repository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TagDto>>(tags));
        }

        // POST create tag
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto dto)
        {
            var tag = _mapper.Map<Tag>(dto);
            await _repository.AddAsync(tag);
            return CreatedAtAction(nameof(GetTags), new { id = tag.Id }, _mapper.Map<TagDto>(tag));
        }

        // PATCH update tag
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PatchTag(Guid id, [FromBody] PatchTagDto dto)
        {
            if (dto == null) return BadRequest();

            var tag = await _repository.GetByIdAsync(id);
            if (tag == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.Name))
                tag.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.ColorHex))
                tag.ColorHex = dto.ColorHex;

            await _repository.UpdateAsync(tag);
            var tagdto = _mapper.Map<TagDto>(tag);
            return Ok(tagdto);
        }

        // DELETE tag
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
