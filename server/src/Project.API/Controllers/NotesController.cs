using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Note;
using Project.APPLICATION.Queries.Note;
using System.Security.Claims;

namespace Project.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/tasks/{taskId}/[controller]")]
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTaskNotes(string taskId)
    {
        try
        {
            var query = new GetTaskNotesQuery(taskId);
            var notes = await _mediator.Send(query);
            return Ok(new { success = true, data = notes });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNoteById(string taskId, string noteId)
    {
        try
        {
            var query = new GetNoteByIdQuery(noteId);
            var note = await _mediator.Send(query);
            return Ok(new { success = true, data = note });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string taskId, [FromBody] CreateNoteCommand command)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            var createCommand = command with { TaskId = taskId, UserId = userId };
            var note = await _mediator.Send(createCommand);
            return Ok(new { success = true, data = note, message = "Note created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{noteId}")]
    public async Task<IActionResult> Update(string taskId, string noteId, [FromBody] UpdateNoteCommand command)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            // Get note to check ownership
            var noteQuery = new GetNoteByIdQuery(noteId);
            var existingNote = await _mediator.Send(noteQuery);
            
            if (existingNote.UserId != userId)
                return Forbid();
            
            var updateCommand = command with { Id = noteId };
            var note = await _mediator.Send(updateCommand);
            return Ok(new { success = true, data = note, message = "Note updated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpDelete("{noteId}")]
    public async Task<IActionResult> Delete(string taskId, string noteId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            // Get note to check ownership
            var noteQuery = new GetNoteByIdQuery(noteId);
            var existingNote = await _mediator.Send(noteQuery);
            
            if (existingNote.UserId != userId)
                return Forbid();
            
            var command = new DeleteNoteCommand(noteId);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Note deleted successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost("{noteId}/attachments")]
    public async Task<IActionResult> UploadAttachment(string taskId, string noteId, IFormFile file)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            // Validate file
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "No file uploaded" });
            
            // Validate file type (images only)
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest(new { success = false, message = "Only image files are allowed" });
            
            // Validate file size (5MB max)
            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new { success = false, message = "File size must be less than 5MB" });
            
            // Get note to check ownership
            var noteQuery = new GetNoteByIdQuery(noteId);
            var existingNote = await _mediator.Send(noteQuery);
            
            if (existingNote.UserId != userId)
                return Forbid();
            
            var command = new UploadNoteAttachmentCommand(
                noteId,
                file.FileName,
                file.ContentType,
                file.Length,
                file.OpenReadStream()
            );
            
            var attachment = await _mediator.Send(command);
            return Ok(new { success = true, data = attachment, message = "File uploaded successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpDelete("{noteId}/attachments/{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment(string taskId, string noteId, string attachmentId)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { success = false, message = "User not authenticated" });
            
            // Get note to check ownership
            var noteQuery = new GetNoteByIdQuery(noteId);
            var existingNote = await _mediator.Send(noteQuery);
            
            if (existingNote.UserId != userId)
                return Forbid();
            
            var command = new DeleteNoteAttachmentCommand(noteId, attachmentId);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Attachment deleted successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
