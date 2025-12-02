using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Task;
using Project.APPLICATION.Queries.Task;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? projectId,
        [FromQuery] string? userId)
    {
        try
        {
            if (!string.IsNullOrEmpty(projectId))
            {
                var query = new GetProjectTasksQuery(projectId);
                var tasks = await _mediator.Send(query);
                return Ok(new { success = true, data = tasks });
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                var query = new GetUserTasksQuery(userId);
                var tasks = await _mediator.Send(query);
                return Ok(new { success = true, data = tasks });
            }
            else
            {
                var query = new GetAllTasksQuery();
                var tasks = await _mediator.Send(query);
                return Ok(new { success = true, data = tasks });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromQuery] bool includeComments = false)
    {
        try
        {
            var query = new GetTaskByIdQuery(id, includeComments);
            var task = await _mediator.Send(query);
            
            if (task == null)
                return NotFound(new { success = false, message = "Task not found" });
            
            return Ok(new { success = true, data = task });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
    {
        try
        {
            var task = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = task.Id },
                new { success = true, data = task, message = "Task created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskCommand command)
    {
        try
        {
            var updateCommand = command with { Id = id };
            var task = await _mediator.Send(updateCommand);
            return Ok(new { success = true, data = task, message = "Task updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var command = new DeleteTaskCommand(id);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Task deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> BulkDelete([FromBody] List<string> taskIds)
    {
        try
        {
            var command = new BulkDeleteTasksCommand(taskIds);
            var deletedCount = await _mediator.Send(command);
            return Ok(new { success = true, message = $"{deletedCount} tasks deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
