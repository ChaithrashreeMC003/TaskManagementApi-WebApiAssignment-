using AutoMapper;
using TaskBoardApi.Models.Domain;
using TaskBoardApi.Models.DTOs;
using TaskBoardApi.Repositories;

namespace TaskBoardApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create
        public async Task<TaskItem?> CreateTaskAsync(CreateTaskItemDto dto, Guid createdByUserId)
        {
            var task = _mapper.Map<TaskItem>(dto);
            task.Id = Guid.NewGuid();

            await _unitOfWork.TaskItems.AddAsync(task);

            if (dto.AssignedToUserId.HasValue)
            {
                var assignment = new TaskAssignment
                {
                    Id = Guid.NewGuid(),
                    TaskItemId = task.Id,
                    AssignedToUserId = dto.AssignedToUserId.Value,
                    AssignedByUserId = createdByUserId,
                    AssignedAt = DateTime.UtcNow,
                    Comment = "Initial assignment"
                };

                await _unitOfWork.TaskAssignments.AddAsync(assignment);
            }

            await _unitOfWork.CompleteAsync();
            return task;
        }

        //  Get All
        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _unitOfWork.TaskItems.GetAllAsync();
        }

        // Get By Id
        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.TaskItems.GetByIdAsync(id);
        }

        // Update (PUT)
        public async Task<TaskItem?> UpdateAsync(Guid id, UpdateTaskItemDto dto)
        {
            var existingTask = await _unitOfWork.TaskItems.GetByIdAsync(id);
            if (existingTask == null) return null;

            // map new values
            _mapper.Map(dto, existingTask);

            _unitOfWork.TaskItems.Update(existingTask);
            await _unitOfWork.CompleteAsync();

            return existingTask;
        }

        // Patch (Partial Update)
        public async Task<TaskItem?> PatchAsync(Guid id, PatchTaskItemDto dto)
        {
            var existingTask = await _unitOfWork.TaskItems.GetByIdAsync(id);
            if (existingTask == null) return null;

            // only update provided fields
            if (dto.Title != null) existingTask.Title = dto.Title;
            if (dto.Description != null) existingTask.Description = dto.Description;
            if (dto.Status.HasValue) existingTask.Status = dto.Status.Value;
            if (dto.Priority.HasValue) existingTask.Priority = dto.Priority.Value;
            if (dto.AssignedToUserId.HasValue) existingTask.AssignedToUserId = dto.AssignedToUserId;

            _unitOfWork.TaskItems.Update(existingTask);
            await _unitOfWork.CompleteAsync();

            return existingTask;
        }

        //  Delete
        public async Task<TaskItem?> DeleteAsync(Guid id)
        {
            var existingTask = await _unitOfWork.TaskItems.GetByIdAsync(id);
            if (existingTask == null) return null;

            _unitOfWork.TaskItems.Delete(existingTask);
            await _unitOfWork.CompleteAsync();

            return existingTask;
        }
    }
}

