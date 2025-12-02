using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.DTOs.Project;
using Project.CORE.Entities;
using Project.CORE.Interfaces;
using Project.CORE.ValueObjects;

namespace Project.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjectsController> _logger;
    
    public ProjectsController(
        IProjectRepository projectRepository,
        IMapper mapper,
        ILogger<ProjectsController> logger)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? workspaceId)
    {
        try
        {
            IEnumerable<ProjectEntity> projects;
            
            if (!string.IsNullOrEmpty(workspaceId))
            {
                projects = await _projectRepository.GetWorkspaceProjectsAsync(workspaceId);
            }
            else
            {
                projects = await _projectRepository.GetAllAsync();
            }
            
            var projectDtos = _mapper.Map<List<ProjectDto>>(projects);
            return Ok(new { success = true, data = projectDtos });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projects");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, [FromQuery] bool includeTasks = false, [FromQuery] bool includeMembers = false)
    {
        try
        {
            ProjectEntity? project;
            
            if (includeTasks)
            {
                project = await _projectRepository.GetWithTasksAsync(id);
            }
            else if (includeMembers)
            {
                project = await _projectRepository.GetWithMembersAsync(id);
            }
            else
            {
                project = await _projectRepository.GetByIdAsync(id);
            }
            
            if (project == null)
                return NotFound(new { success = false, message = "Project not found" });
            
            var projectDto = includeTasks || includeMembers 
                ? _mapper.Map<ProjectDetailDto>(project)
                : (object)_mapper.Map<ProjectDto>(project);
            
            return Ok(new { success = true, data = projectDto });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto request)
    {
        try
        {
            var project = new ProjectEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Priority = Enum.Parse<Priority>(request.Priority),
                Status = Enum.Parse<ProjectStatus>(request.Status),
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TeamLeadId = request.TeamLeadId,
                WorkspaceId = request.WorkspaceId,
                Progress = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            var created = await _projectRepository.AddAsync(project);
            var projectDto = _mapper.Map<ProjectDto>(created);
            
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, 
                new { success = true, data = projectDto, message = "Project created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProjectDto request)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project == null)
                return NotFound(new { success = false, message = "Project not found" });
            
            project.Name = request.Name;
            project.Description = request.Description;
            project.Priority = Enum.Parse<Priority>(request.Priority);
            project.Status = Enum.Parse<ProjectStatus>(request.Status);
            
            if (request.StartDate.HasValue)
                project.StartDate = request.StartDate.Value;
            
            if (request.EndDate.HasValue)
                project.EndDate = request.EndDate.Value;
            
            if (request.Progress.HasValue)
                project.Progress = request.Progress.Value;
            
            await _projectRepository.UpdateAsync(project);
            var projectDto = _mapper.Map<ProjectDto>(project);
            
            return Ok(new { success = true, data = projectDto, message = "Project updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _projectRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {Id}", id);
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}
