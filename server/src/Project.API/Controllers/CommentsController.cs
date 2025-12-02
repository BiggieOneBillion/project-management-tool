using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Comment;
using Project.APPLICATION.Queries.Comment;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/tasks/{taskId}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTaskComments(string taskId)
    {
        try
        {
            var query = new GetTaskCommentsQuery(taskId);
            var comments = await _mediator.Send(query);
            return Ok(new { success = true, data = comments });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string taskId, [FromBody] CreateCommentCommand command)
    {
        try
        {
            // Ensure taskId from route is used
            var createCommand = command with { TaskId = taskId };
            var comment = await _mediator.Send(createCommand);
            return Ok(new { success = true, data = comment, message = "Comment created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{commentId}")]
    public async Task<IActionResult> Update(string taskId, string commentId, [FromBody] UpdateCommentCommand command)
    {
        try
        {
            var updateCommand = command with { Id = commentId };
            var comment = await _mediator.Send(updateCommand);
            return Ok(new { success = true, data = comment, message = "Comment updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpDelete("{commentId}")]
    public async Task<IActionResult> Delete(string taskId, string commentId)
    {
        try
        {
            var command = new DeleteCommentCommand(commentId);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Comment deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
