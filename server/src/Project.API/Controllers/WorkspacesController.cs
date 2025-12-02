using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Entities;
using Project.CORE.Interfaces;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkspacesController> _logger;
    
    public WorkspacesController(
        IWorkspaceRepository workspaceRepository,
        IMapper mapper,
        ILogger<WorkspacesController> logger)
    {
        _workspaceRepository = workspaceRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? userId)
    {
        try
        {
            IEnumerable<Workspace> workspaces;
            
            if (!string.IsNullOrEmpty(userId))
            {
                workspaces = await _workspaceRepository.GetUserWorkspacesAsync(userId);
            }
            else
            {
                workspaces = await _workspaceRepository.GetAllAsync();
            }
            
            var workspaceDtos = _mapper.Map<List<WorkspaceDto>>(workspaces);
            return Ok(new { success = true, data = workspaceDtos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workspaces");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromQuery] bool includeMembers = false, [FromQuery] bool includeProjects = false)
    {
        try
        {
            Workspace? workspace;
            
            if (includeMembers)
            {
                workspace = await _workspaceRepository.GetWithMembersAsync(id);
            }
            else if (includeProjects)
            {
                workspace = await _workspaceRepository.GetWithProjectsAsync(id);
            }
            else
            {
                workspace = await _workspaceRepository.GetByIdAsync(id);
            }
            
            if (workspace == null)
                return NotFound(new { success = false, message = "Workspace not found" });
            
            var workspaceDto = includeMembers || includeProjects
                ? _mapper.Map<WorkspaceDetailDto>(workspace)
                : (object)_mapper.Map<WorkspaceDto>(workspace);
            
            return Ok(new { success = true, data = workspaceDto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workspace {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkspaceDto request)
    {
        try
        {
            var workspace = new Workspace
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                OwnerId = request.OwnerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var created = await _workspaceRepository.AddAsync(workspace);
            var workspaceDto = _mapper.Map<WorkspaceDto>(created);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, 
                new { success = true, data = workspaceDto, message = "Workspace created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating workspace");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateWorkspaceDto request)
    {
        try
        {
            var workspace = await _workspaceRepository.GetByIdAsync(id);
            
            if (workspace == null)
                return NotFound(new { success = false, message = "Workspace not found" });
            
            workspace.Name = request.Name;
            workspace.Description = request.Description;
            workspace.Settings = request.Settings ?? workspace.Settings;
            
            await _workspaceRepository.UpdateAsync(workspace);
            var workspaceDto = _mapper.Map<WorkspaceDto>(workspace);
            
            return Ok(new { success = true, data = workspaceDto, message = "Workspace updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workspace {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _workspaceRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting workspace {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}

