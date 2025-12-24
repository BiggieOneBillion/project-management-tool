using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Project;
using Project.CORE.Entities;
using Project.CORE.Exceptions;
using Project.CORE.Interfaces;
using Project.CORE.ValueObjects;

namespace Project.APPLICATION.Commands.Project;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public CreateProjectCommandHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
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
        
        var created = await _repository.AddAsync(project);
        return _mapper.Map<ProjectDto>(created);
    }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    
    public UpdateProjectCommandHandler(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.Id);
        
        if (project == null)
            throw new EntityNotFoundException("Project", request.Id);
        
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
        
        project.UpdatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(project);
        return _mapper.Map<ProjectDto>(project);
    }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
{
    private readonly IProjectRepository _repository;
    
    public DeleteProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}

public class AddProjectMemberCommandHandler : IRequestHandler<AddProjectMemberCommand, ProjectDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public AddProjectMemberCommandHandler(
        IProjectRepository projectRepository, 
        IUserRepository userRepository,
        IMapper mapper)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<ProjectDto> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
    {
        // Get the project
        var project = await _projectRepository.GetWithMembersAsync(request.ProjectId);
        if (project == null)
            throw new EntityNotFoundException("Project", request.ProjectId);
        
        // Get the user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new EntityNotFoundException("User with email", request.Email);
        
        // Check if user is already a member
        if (project.Members.Any(m => m.UserId == user.Id))
            throw new InvalidOperationException($"User {request.Email} is already a member of this project");
        
        // Add the member
        var projectMember = new ProjectMember
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            ProjectId = request.ProjectId,
            AddedAt = DateTime.UtcNow
        };
        
        project.Members.Add(projectMember);
        await _projectRepository.UpdateAsync(project);
        
        // Return updated project with members
        var updatedProject = await _projectRepository.GetWithMembersAsync(request.ProjectId);
        return _mapper.Map<ProjectDto>(updatedProject);
    }
}
