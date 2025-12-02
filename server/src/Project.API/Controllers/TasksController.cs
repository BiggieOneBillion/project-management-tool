using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.DTOs.Task;
using Project.CORE.Entities;
using Project.CORE.Interfaces;
using Project.CORE.ValueObjects;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TasksController> _logger;
    
    public TasksController(
        ITaskRepository taskRepository,
        IMapper mapper,
        ILogger<TasksController> logger)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? projectId, [FromQuery] string? userId)
    {
        try
        {
            IEnumerable<TaskEntity> tasks;
            
            if (!string.IsNullOrEmpty(projectId))
            {
                tasks = await _taskRepository.GetProjectTasksAsync(projectId);
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                tasks = await _taskRepository.GetUserTasksAsync(userId);
            }
            else
            {
                tasks = await _taskRepository.GetAllAsync();
            }
            
            var taskDtos = _mapper.Map<List<TaskDto>>(tasks);
            return Ok(new { success = true, data = taskDtos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tasks");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromQuery] bool includeComments = false)
    {
        try
        {
            TaskEntity? task;
            
            if (includeComments)
            {
                task = await _taskRepository.GetWithCommentsAsync(id);
            }
            else
            {
                task = await _taskRepository.GetByIdAsync(id);
            }
            
            if (task == null)
                return NotFound(new { success = false, message = "Task not found" });
            
            var taskDto = includeComments 
                ? _mapper.Map<TaskDetailDto>(task)
                : (object)_mapper.Map<TaskDto>(task);
            
            return Ok(new { success = true, data = taskDto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto request)
    {
        try
        {
            var task = new TaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                ProjectId = request.ProjectId,
                Title = request.Title,
                Description = request.Description,
                Status = Enum.Parse<CORE.ValueObjects.TaskStatus>(request.Status),
                Type = Enum.Parse<TaskType>(request.Type),
                Priority = Enum.Parse<Priority>(request.Priority),
                AssigneeId = request.AssigneeId,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var created = await _taskRepository.AddAsync(task);
            var taskDto = _mapper.Map<TaskDto>(created);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, 
                new { success = true, data = taskDto, message = "Task created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskDto request)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(id);
            
            if (task == null)
                return NotFound(new { success = false, message = "Task not found" });
            
            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = Enum.Parse<CORE.ValueObjects.TaskStatus>(request.Status);
            task.Type = Enum.Parse<TaskType>(request.Type);
            task.Priority = Enum.Parse<Priority>(request.Priority);
            task.AssigneeId = request.AssigneeId;
            
            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate.Value;
            
            await _taskRepository.UpdateAsync(task);
            var taskDto = _mapper.Map<TaskDto>(task);
            
            return Ok(new { success = true, data = taskDto, message = "Task updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> BulkDelete([FromBody] List<string> taskIds)
    {
        try
        {
            foreach (var taskId in taskIds)
            {
                await _taskRepository.DeleteAsync(taskId);
            }
            
            return Ok(new { success = true, message = $"{taskIds.Count} tasks deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk deleting tasks");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}
