using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Project;
using Project.APPLICATION.Queries.Project;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? workspaceId, [FromQuery] bool includeTasks = false)
    {
        try
        {
            if (!string.IsNullOrEmpty(workspaceId))
            {
                var query = new GetWorkspaceProjectsQuery(workspaceId, includeTasks);
                var projects = await _mediator.Send(query);
                return Ok(new { success = true, data = projects });
            }
            else
            {
                var query = new GetAllProjectsQuery();
                var projects = await _mediator.Send(query);
                return Ok(new { success = true, data = projects });
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
        [FromQuery] bool includeTasks = false,
        [FromQuery] bool includeMembers = false)
    {
        try
        {
            var query = new GetProjectByIdQuery(id, includeTasks, includeMembers);
            var project = await _mediator.Send(query);
            
            if (project == null)
                return NotFound(new { success = false, message = "Project not found" });
            
            return Ok(new { success = true, data = project });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command)
    {
        try
        {
            var project = await _mediator.Send(command);
            return CreatedAtAction(
                nameof(GetById),
                new { id = project.Id },
                new { success = true, data = project, message = "Project created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProjectCommand command)
    {
        try
        {
            var updateCommand = command with { Id = id };
            var project = await _mediator.Send(updateCommand);
            return Ok(new { success = true, data = project, message = "Project updated successfully" });
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
            var command = new DeleteProjectCommand(id);
            await _mediator.Send(command);
            return Ok(new { success = true, message = "Project deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetProjectMembers(string id)
    {
        try
        {
            var query = new GetProjectByIdQuery(Id:id, IncludeTasks: false, IncludeMembers: true);
            var project = await _mediator.Send(query);
            
            if (project == null)
                return NotFound(new { success = false, message = "Project not found" });
            
            return Ok(new { success = true, data = project.Members });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{id}/members/list")]
    public async Task<IActionResult> GetProjectMembersList(string id)
    {
        try
        {
            var query = new GetProjectMembersQuery(id);
            var members = await _mediator.Send(query);
            return Ok(new { success = true, data = members });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpGet("{id}/available-members")]
    public async Task<IActionResult> GetAvailableWorkspaceMembers(string id, [FromQuery] string workspaceId)
    {
        try
        {
            if (string.IsNullOrEmpty(workspaceId))
                return BadRequest(new { success = false, message = "WorkspaceId is required" });
            
            var query = new GetAvailableWorkspaceMembersQuery(id, workspaceId);
            var members = await _mediator.Send(query);
            return Ok(new { success = true, data = members });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
    
    [HttpPost("{id}/members")]
    public async Task<IActionResult> AddProjectMember(string id, [FromBody] AddProjectMemberRequest request)
    {
        try
        {
            var command = new AddProjectMemberCommand(id, request.Email);
            var project = await _mediator.Send(command);
            return Ok(new { success = true, data = project, message = "Member added successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}

public record AddProjectMemberRequest(string Email);
