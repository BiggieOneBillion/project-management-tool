using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Workspace;
using Project.APPLICATION.Queries.Workspace;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public WorkspacesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? userId)
    {
        try
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var query = new GetUserWorkspacesQuery(userId);
                var workspaces = await _mediator.Send(query);
                return Ok(new { success = true, data = workspaces });
            }
            else
            {
                var query = new GetAllWorkspacesQuery();
                var workspaces = await _mediator.Send(query);
                return Ok(new { success = true, data = workspaces });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        string id,
        [FromQuery] bool includeMembers = false,
        [FromQuery] bool includeProjects = false)
    {
        try
        {
            var query = new GetWorkspaceByIdQuery(id, includeMembers, includeProjects);
            var workspace = await _mediator.Send(query);
            
            if (workspace == null)
                return NotFound(new { success = false, message = "Workspace not found" });
            
            return Ok(new { success = true, data = workspace });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkspaceCommand command)
    {
        try
        {
            var workspace = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = workspace.Id },
                new { success = true, data = workspace, message = "Workspace created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateWorkspaceCommand command)
    {
        try
        {
            // Ensure the ID in the route matches the command
            var updateCommand = command with { Id = id };
            var workspace = await _mediator.Send(updateCommand);
            return Ok(new { success = true, data = workspace, message = "Workspace updated successfully" });
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
            var command = new DeleteWorkspaceCommand(id);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Workspace deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
