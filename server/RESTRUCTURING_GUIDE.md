# Backend Restructuring Guide

This guide explains how to restructure the backend to match BACKEND_ARCHITECTURE.md exactly.

## Overview

The restructuring involves:
1. Reorganizing DTOs into entity-specific folders
2. Implementing CQRS with MediatR (Commands, Queries, Handlers)
3. Creating Application Services
4. Adding FluentValidation validators
5. Separating repositories into individual files
6. Adding Domain Aggregates and Specifications

## Current vs Target Structure

### Current Structure
```
Project.APPLICATION/
├── DTOs/
│   └── DTOs.cs (all DTOs in one file)
├── Mappings/
│   └── MappingProfile.cs
└── DependencyInjection.cs
```

### Target Structure
```
Project.APPLICATION/
├── Services/
│   ├── WorkspaceService.cs
│   ├── ProjectService.cs
│   ├── TaskService.cs
│   └── UserService.cs
├── DTOs/
│   ├── Workspace/
│   │   ├── WorkspaceDto.cs
│   │   ├── CreateWorkspaceDto.cs
│   │   └── UpdateWorkspaceDto.cs
│   ├── Project/
│   ├── Task/
│   └── User/
├── Commands/
│   ├── Workspace/
│   │   ├── CreateWorkspaceCommand.cs
│   │   └── CreateWorkspaceCommandHandler.cs
│   ├── Project/
│   └── Task/
├── Queries/
│   ├── Workspace/
│   │   ├── GetWorkspaceByIdQuery.cs
│   │   └── GetWorkspaceByIdQueryHandler.cs
│   ├── Project/
│   └── Task/
├── Validators/
│   ├── CreateWorkspaceValidator.cs
│   ├── CreateProjectValidator.cs
│   └── CreateTaskValidator.cs
├── Mappings/
│   └── MappingProfile.cs
├── Interfaces/
│   ├── IWorkspaceService.cs
│   ├── IProjectService.cs
│   └── ITaskService.cs
└── Exceptions/
    ├── ApplicationException.cs
    └── ValidationException.cs
```

## Step-by-Step Restructuring

### Phase 1: APPLICATION Layer - DTOs

#### 1.1 Create Workspace DTOs
Create folder: `Project.APPLICATION/DTOs/Workspace/`

**WorkspaceDto.cs**:
```csharp
namespace Project.APPLICATION.DTOs.Workspace;

public record WorkspaceDto(
    string Id,
    string Name,
    string Slug,
    string? Description,
    string? ImageUrl,
    string OwnerId,
    int MemberCount,
    int ProjectCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
```

**CreateWorkspaceDto.cs**:
```csharp
namespace Project.APPLICATION.DTOs.Workspace;

public record CreateWorkspaceDto(
    string Name,
    string Slug,
    string? Description,
    string OwnerId
);
```

**UpdateWorkspaceDto.cs**:
```csharp
namespace Project.APPLICATION.DTOs.Workspace;

public record UpdateWorkspaceDto(
    string Name,
    string? Description,
    string? Settings
);
```

#### 1.2 Repeat for Project, Task, User DTOs
Follow the same pattern for each entity.

### Phase 2: APPLICATION Layer - CQRS

#### 2.1 Create Workspace Commands

**Commands/Workspace/CreateWorkspaceCommand.cs**:
```csharp
using MediatR;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Commands.Workspace;

public record CreateWorkspaceCommand(
    string Name,
    string Slug,
    string? Description,
    string OwnerId
) : IRequest<WorkspaceDto>;
```

**Commands/Workspace/CreateWorkspaceCommandHandler.cs**:
```csharp
using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Workspace;

public class CreateWorkspaceCommandHandler 
    : IRequestHandler<CreateWorkspaceCommand, WorkspaceDto>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public CreateWorkspaceCommandHandler(
        IWorkspaceRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceDto> Handle(
        CreateWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        var workspace = new CORE.Entities.Workspace
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var created = await _repository.AddAsync(workspace);
        return _mapper.Map<WorkspaceDto>(created);
    }
}
```

#### 2.2 Create Workspace Queries

**Queries/Workspace/GetWorkspaceByIdQuery.cs**:
```csharp
using MediatR;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Queries.Workspace;

public record GetWorkspaceByIdQuery(string Id) : IRequest<WorkspaceDto?>;
```

**Queries/Workspace/GetWorkspaceByIdQueryHandler.cs**:
```csharp
using AutoMapper;
using MediatR;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Queries.Workspace;

public class GetWorkspaceByIdQueryHandler 
    : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto?>
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public GetWorkspaceByIdQueryHandler(
        IWorkspaceRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceDto?> Handle(
        GetWorkspaceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var workspace = await _repository.GetByIdAsync(request.Id);
        return workspace == null ? null : _mapper.Map<WorkspaceDto>(workspace);
    }
}
```

### Phase 3: APPLICATION Layer - Validators

**Validators/CreateWorkspaceValidator.cs**:
```csharp
using FluentValidation;
using Project.APPLICATION.Commands.Workspace;

namespace Project.APPLICATION.Validators;

public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
        
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric with hyphens")
            .MaximumLength(200);
        
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("Owner is required");
    }
}
```

### Phase 4: APPLICATION Layer - Services

**Interfaces/IWorkspaceService.cs**:
```csharp
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Interfaces;

public interface IWorkspaceService
{
    Task<WorkspaceDto> CreateAsync(CreateWorkspaceDto dto);
    Task<WorkspaceDto?> GetByIdAsync(string id);
    Task<IEnumerable<WorkspaceDto>> GetAllAsync();
    Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceDto dto);
    Task DeleteAsync(string id);
}
```

**Services/WorkspaceService.cs**:
```csharp
using AutoMapper;
using Project.APPLICATION.DTOs.Workspace;
using Project.APPLICATION.Interfaces;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _repository;
    private readonly IMapper _mapper;
    
    public WorkspaceService(IWorkspaceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<WorkspaceDto> CreateAsync(CreateWorkspaceDto dto)
    {
        var workspace = new CORE.Entities.Workspace
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            OwnerId = dto.OwnerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var created = await _repository.AddAsync(workspace);
        return _mapper.Map<WorkspaceDto>(created);
    }
    
    public async Task<WorkspaceDto?> GetByIdAsync(string id)
    {
        var workspace = await _repository.GetByIdAsync(id);
        return workspace == null ? null : _mapper.Map<WorkspaceDto>(workspace);
    }
    
    public async Task<IEnumerable<WorkspaceDto>> GetAllAsync()
    {
        var workspaces = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
    }
    
    public async Task<WorkspaceDto> UpdateAsync(string id, UpdateWorkspaceDto dto)
    {
        var workspace = await _repository.GetByIdAsync(id);
        if (workspace == null)
            throw new KeyNotFoundException($"Workspace {id} not found");
        
        workspace.Name = dto.Name;
        workspace.Description = dto.Description;
        workspace.Settings = dto.Settings ?? workspace.Settings;
        
        await _repository.UpdateAsync(workspace);
        return _mapper.Map<WorkspaceDto>(workspace);
    }
    
    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }
}
```

### Phase 5: Update Controllers to Use MediatR

**Controllers/WorkspacesController.cs** (Updated):
```csharp
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var query = new GetWorkspaceByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (result == null)
            return NotFound(new { success = false, message = "Workspace not found" });
        
        return Ok(new { success = true, data = result });
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkspaceCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, 
            new { success = true, data = result, message = "Workspace created successfully" });
    }
}
```

### Phase 6: Update DependencyInjection

**Project.APPLICATION/DependencyInjection.cs**:
```csharp
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Project.APPLICATION.Interfaces;
using Project.APPLICATION.Services;
using System.Reflection;

namespace Project.APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Services
        services.AddScoped<IWorkspaceService, WorkspaceService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}
```

## Files to Create

### Complete File List

#### Project.CORE
- ✅ ValueObjects/Email.cs
- ✅ ValueObjects/Priority.cs
- ✅ ValueObjects/ProjectStatus.cs
- ✅ ValueObjects/TaskStatus.cs
- ✅ ValueObjects/TaskType.cs
- ✅ Exceptions/DomainException.cs
- ✅ Exceptions/EntityNotFoundException.cs
- ✅ Exceptions/BusinessRuleViolationException.cs
- ✅ DomainEvents/DomainEvent.cs
- ✅ DomainEvents/TaskAssignedEvent.cs
- ✅ DomainEvents/ProjectCreatedEvent.cs
- ✅ DomainEvents/WorkspaceMemberAddedEvent.cs
- [ ] Aggregates/WorkspaceAggregate.cs
- [ ] Aggregates/ProjectAggregate.cs
- [ ] DomainServices/TaskAssignmentService.cs
- [ ] Specifications/TaskSpecifications.cs

#### Project.APPLICATION
- [ ] DTOs/Workspace/WorkspaceDto.cs
- [ ] DTOs/Workspace/CreateWorkspaceDto.cs
- [ ] DTOs/Workspace/UpdateWorkspaceDto.cs
- [ ] DTOs/Project/* (similar structure)
- [ ] DTOs/Task/* (similar structure)
- [ ] DTOs/User/* (similar structure)
- [ ] Commands/Workspace/CreateWorkspaceCommand.cs
- [ ] Commands/Workspace/CreateWorkspaceCommandHandler.cs
- [ ] Commands/Workspace/UpdateWorkspaceCommand.cs
- [ ] Commands/Workspace/UpdateWorkspaceCommandHandler.cs
- [ ] Commands/Workspace/DeleteWorkspaceCommand.cs
- [ ] Commands/Workspace/DeleteWorkspaceCommandHandler.cs
- [ ] Queries/Workspace/GetWorkspaceByIdQuery.cs
- [ ] Queries/Workspace/GetWorkspaceByIdQueryHandler.cs
- [ ] Queries/Workspace/GetAllWorkspacesQuery.cs
- [ ] Queries/Workspace/GetAllWorkspacesQueryHandler.cs
- [ ] Validators/CreateWorkspaceValidator.cs
- [ ] Validators/UpdateWorkspaceValidator.cs
- [ ] Services/WorkspaceService.cs
- [ ] Interfaces/IWorkspaceService.cs
- [ ] Exceptions/ApplicationException.cs
- [ ] Exceptions/ValidationException.cs

#### Project.INFRASTRUCTURE
- [ ] Repositories/WorkspaceRepository.cs (separate file)
- [ ] Repositories/ProjectRepository.cs (separate file)
- [ ] Repositories/TaskRepository.cs (separate file)
- [ ] Repositories/UserRepository.cs (separate file)
- [ ] Services/EmailService.cs
- [ ] Services/FileStorageService.cs
- [ ] Services/CacheService.cs

## Estimated Effort

- **Total Files to Create**: ~80 files
- **Estimated Time**: 4-6 hours for complete restructuring
- **Complexity**: Medium-High

## Recommendation

Given the extensive nature of this restructuring, I recommend:

1. **Option A**: Keep current working structure and gradually refactor
2. **Option B**: Create a new branch and do complete restructuring
3. **Option C**: Use the current simplified structure (it works!) and add CQRS incrementally

The current backend is **fully functional** with all CRUD operations. The CQRS pattern adds complexity but provides better separation of concerns for larger teams.

Would you like me to:
1. Continue with full restructuring (will create all ~80 files)
2. Create a hybrid approach (keep current structure, add MediatR to controllers)
3. Provide scripts to automate the restructuring?
