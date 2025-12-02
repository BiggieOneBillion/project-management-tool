using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.DTOs.Comment;
using Project.CORE.Entities;
using Project.CORE.Interfaces;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/tasks/{taskId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly IRepository<Comment> _commentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentsController> _logger;
    
    public CommentsController(
        IRepository<Comment> commentRepository,
        IMapper mapper,
        ILogger<CommentsController> logger)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTaskComments(string taskId)
    {
        try
        {
            var allComments = await _commentRepository.GetAllAsync();
            var taskComments = allComments.Where(c => c.TaskId == taskId).ToList();
            var commentDtos = _mapper.Map<List<CommentDto>>(taskComments);
            
            return Ok(new { success = true, data = commentDtos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting comments for task {TaskId}", taskId);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string taskId, [FromBody] CreateCommentDto request)
    {
        try
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                TaskId = taskId,
                UserId = request.TaskId, // This should be from authenticated user context
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var created = await _commentRepository.AddAsync(comment);
            var commentDto = _mapper.Map<CommentDto>(created);
            
            return CreatedAtAction(nameof(GetTaskComments), new { taskId }, 
                new { success = true, data = commentDto, message = "Comment created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating comment");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPut("{commentId}")]
    public async Task<IActionResult> Update(string taskId, string commentId, [FromBody] UpdateCommentDto request)
    {
        try
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            
            if (comment == null)
                return NotFound(new { success = false, message = "Comment not found" });
            
            if (comment.TaskId != taskId)
                return BadRequest(new { success = false, message = "Comment does not belong to this task" });
            
            comment.Content = request.Content;
            
            await _commentRepository.UpdateAsync(comment);
            var commentDto = _mapper.Map<CommentDto>(comment);
            
            return Ok(new { success = true, data = commentDto, message = "Comment updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating comment {Id}", commentId);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpDelete("{commentId}")]
    public async Task<IActionResult> Delete(string taskId, string commentId)
    {
        try
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            
            if (comment == null)
                return NotFound(new { success = false, message = "Comment not found" });
            
            if (comment.TaskId != taskId)
                return BadRequest(new { success = false, message = "Comment does not belong to this task" });
            
            await _commentRepository.DeleteAsync(commentId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting comment {Id}", commentId);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}
