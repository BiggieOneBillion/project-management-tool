# Backend Restructuring - Stage-by-Stage Plan

## Overview

This document outlines the complete restructuring of the backend to match BACKEND_ARCHITECTURE.md specifications. The restructuring is divided into 6 stages that can be executed independently.

## Progress Tracker

- [x] **Stage 0**: Planning and Foundation (COMPLETED)
- [ ] **Stage 1**: Complete CORE Layer
- [ ] **Stage 2**: Restructure APPLICATION DTOs
- [ ] **Stage 3**: Implement Commands and Handlers
- [ ] **Stage 4**: Implement Queries and Handlers
- [ ] **Stage 5**: Create Services and Validators
- [ ] **Stage 6**: Update Controllers and Infrastructure

---

## Stage 0: Planning and Foundation ✅

**Status**: COMPLETED

**What was done**:
- ✅ Created Value Objects (Email, Priority, ProjectStatus, TaskStatus, TaskType)
- ✅ Created Domain Exceptions (DomainException, EntityNotFoundException, BusinessRuleViolationException)
- ✅ Created Domain Events (TaskAssignedEvent, ProjectCreatedEvent, WorkspaceMemberAddedEvent)
- ✅ Created base DomainEvent class

**Files Created** (12 files):
1. `Project.CORE/ValueObjects/Email.cs`
2. `Project.CORE/ValueObjects/Priority.cs`
3. `Project.CORE/ValueObjects/ProjectStatus.cs`
4. `Project.CORE/ValueObjects/TaskStatus.cs`
5. `Project.CORE/ValueObjects/TaskType.cs`
6. `Project.CORE/Exceptions/DomainException.cs`
7. `Project.CORE/Exceptions/EntityNotFoundException.cs`
8. `Project.CORE/Exceptions/BusinessRuleViolationException.cs`
9. `Project.CORE/DomainEvents/DomainEvent.cs`
10. `Project.CORE/DomainEvents/TaskAssignedEvent.cs`
11. `Project.CORE/DomainEvents/ProjectCreatedEvent.cs`
12. `Project.CORE/DomainEvents/WorkspaceMemberAddedEvent.cs`

---

## Stage 1: Complete CORE Layer

**Estimated Time**: 30 minutes  
**Files to Create**: 8 files  
**Status**: PENDING

### Objectives
1. Create Aggregates (WorkspaceAggregate, ProjectAggregate)
2. Create Domain Services (TaskAssignmentService)
3. Create Specifications (TaskSpecifications)
4. Separate Repository interfaces into individual files

### Files to Create

#### Aggregates
1. `Project.CORE/Aggregates/WorkspaceAggregate.cs`
2. `Project.CORE/Aggregates/ProjectAggregate.cs`

#### Domain Services
3. `Project.CORE/DomainServices/TaskAssignmentService.cs`

#### Specifications
4. `Project.CORE/Specifications/ISpecification.cs`
5. `Project.CORE/Specifications/TaskSpecifications.cs`

#### Repository Interfaces (Separate Files)
6. `Project.CORE/Interfaces/IWorkspaceRepository.cs` (move from IRepositories.cs)
7. `Project.CORE/Interfaces/IProjectRepository.cs` (move from IRepositories.cs)
8. `Project.CORE/Interfaces/ITaskRepository.cs` (move from IRepositories.cs)
9. `Project.CORE/Interfaces/IUserRepository.cs` (move from IRepositories.cs)

### Verification
- [ ] All files compile without errors
- [ ] No breaking changes to existing code
- [ ] Repository interfaces properly separated

---

## Stage 2: Restructure APPLICATION DTOs

**Estimated Time**: 45 minutes  
**Files to Create**: 24 files  
**Status**: PENDING

### Objectives
1. Create entity-specific DTO folders
2. Split DTOs.cs into individual files
3. Organize DTOs by entity (Workspace, Project, Task, User, Comment)

### Folder Structure
```
Project.APPLICATION/DTOs/
├── Workspace/
│   ├── WorkspaceDto.cs
│   ├── WorkspaceDetailDto.cs
│   ├── CreateWorkspaceDto.cs
│   └── UpdateWorkspaceDto.cs
├── Project/
│   ├── ProjectDto.cs
│   ├── ProjectDetailDto.cs
│   ├── CreateProjectDto.cs
│   └── UpdateProjectDto.cs
├── Task/
│   ├── TaskDto.cs
│   ├── TaskDetailDto.cs
│   ├── CreateTaskDto.cs
│   └── UpdateTaskDto.cs
├── User/
│   ├── UserDto.cs
│   ├── CreateUserDto.cs
│   └── UpdateUserDto.cs
└── Comment/
    ├── CommentDto.cs
    ├── CreateCommentDto.cs
    └── UpdateCommentDto.cs
```

### Files to Create (24 files)

#### Workspace DTOs (4 files)
1. `Project.APPLICATION/DTOs/Workspace/WorkspaceDto.cs`
2. `Project.APPLICATION/DTOs/Workspace/WorkspaceDetailDto.cs`
3. `Project.APPLICATION/DTOs/Workspace/CreateWorkspaceDto.cs`
4. `Project.APPLICATION/DTOs/Workspace/UpdateWorkspaceDto.cs`

#### Project DTOs (4 files)
5. `Project.APPLICATION/DTOs/Project/ProjectDto.cs`
6. `Project.APPLICATION/DTOs/Project/ProjectDetailDto.cs`
7. `Project.APPLICATION/DTOs/Project/CreateProjectDto.cs`
8. `Project.APPLICATION/DTOs/Project/UpdateProjectDto.cs`

#### Task DTOs (4 files)
9. `Project.APPLICATION/DTOs/Task/TaskDto.cs`
10. `Project.APPLICATION/DTOs/Task/TaskDetailDto.cs`
11. `Project.APPLICATION/DTOs/Task/CreateTaskDto.cs`
12. `Project.APPLICATION/DTOs/Task/UpdateTaskDto.cs`

#### User DTOs (3 files)
13. `Project.APPLICATION/DTOs/User/UserDto.cs`
14. `Project.APPLICATION/DTOs/User/CreateUserDto.cs`
15. `Project.APPLICATION/DTOs/User/UpdateUserDto.cs`

#### Comment DTOs (3 files)
16. `Project.APPLICATION/DTOs/Comment/CommentDto.cs`
17. `Project.APPLICATION/DTOs/Comment/CreateCommentDto.cs`
18. `Project.APPLICATION/DTOs/Comment/UpdateCommentDto.cs`

#### Shared DTOs (6 files)
19. `Project.APPLICATION/DTOs/Workspace/WorkspaceMemberDto.cs`
20. `Project.APPLICATION/DTOs/Project/ProjectMemberDto.cs`
21. `Project.APPLICATION/DTOs/Common/ApiResponse.cs`
22. `Project.APPLICATION/DTOs/Common/PaginatedResult.cs`
23. `Project.APPLICATION/DTOs/Common/ErrorResponse.cs`
24. `Project.APPLICATION/DTOs/Common/SuccessResponse.cs`

### Actions
- [ ] Create all DTO files
- [ ] Update MappingProfile.cs to use new DTO namespaces
- [ ] Delete old DTOs.cs file
- [ ] Verify AutoMapper mappings work

---

## Stage 3: Implement Commands and Handlers

**Estimated Time**: 60 minutes  
**Files to Create**: 30 files  
**Status**: PENDING

### Objectives
1. Create Commands for all entities
2. Create Command Handlers using MediatR
3. Implement business logic in handlers

### Folder Structure
```
Project.APPLICATION/Commands/
├── Workspace/
│   ├── CreateWorkspaceCommand.cs
│   ├── CreateWorkspaceCommandHandler.cs
│   ├── UpdateWorkspaceCommand.cs
│   ├── UpdateWorkspaceCommandHandler.cs
│   ├── DeleteWorkspaceCommand.cs
│   └── DeleteWorkspaceCommandHandler.cs
├── Project/
├── Task/
├── User/
└── Comment/
```

### Files to Create (30 files)

#### Workspace Commands (6 files)
1. `Commands/Workspace/CreateWorkspaceCommand.cs`
2. `Commands/Workspace/CreateWorkspaceCommandHandler.cs`
3. `Commands/Workspace/UpdateWorkspaceCommand.cs`
4. `Commands/Workspace/UpdateWorkspaceCommandHandler.cs`
5. `Commands/Workspace/DeleteWorkspaceCommand.cs`
6. `Commands/Workspace/DeleteWorkspaceCommandHandler.cs`

#### Project Commands (6 files)
7-12. Similar structure for Project

#### Task Commands (8 files - includes BulkDelete)
13-20. Similar structure for Task + BulkDeleteTasksCommand

#### User Commands (6 files)
21-26. Similar structure for User

#### Comment Commands (4 files)
27-30. Create, Update, Delete for Comment

### Verification
- [ ] All commands implement IRequest<TResponse>
- [ ] All handlers implement IRequestHandler<TCommand, TResponse>
- [ ] Handlers use repositories correctly
- [ ] AutoMapper used for entity-to-DTO conversion

---

## Stage 4: Implement Queries and Handlers

**Estimated Time**: 45 minutes  
**Files to Create**: 24 files  
**Status**: PENDING

### Objectives
1. Create Queries for all entities
2. Create Query Handlers using MediatR
3. Implement data retrieval logic

### Folder Structure
```
Project.APPLICATION/Queries/
├── Workspace/
│   ├── GetWorkspaceByIdQuery.cs
│   ├── GetWorkspaceByIdQueryHandler.cs
│   ├── GetAllWorkspacesQuery.cs
│   ├── GetAllWorkspacesQueryHandler.cs
│   ├── GetUserWorkspacesQuery.cs
│   └── GetUserWorkspacesQueryHandler.cs
├── Project/
├── Task/
├── User/
└── Comment/
```

### Files to Create (24 files)

#### Workspace Queries (6 files)
1. `Queries/Workspace/GetWorkspaceByIdQuery.cs`
2. `Queries/Workspace/GetWorkspaceByIdQueryHandler.cs`
3. `Queries/Workspace/GetAllWorkspacesQuery.cs`
4. `Queries/Workspace/GetAllWorkspacesQueryHandler.cs`
5. `Queries/Workspace/GetUserWorkspacesQuery.cs`
6. `Queries/Workspace/GetUserWorkspacesQueryHandler.cs`

#### Project Queries (6 files)
7-12. GetById, GetAll, GetByWorkspace

#### Task Queries (8 files)
13-20. GetById, GetAll, GetByProject, GetByUser

#### User Queries (2 files)
21-22. GetById, GetAll

#### Comment Queries (2 files)
23-24. GetByTaskId

### Verification
- [ ] All queries implement IRequest<TResponse>
- [ ] All handlers implement IRequestHandler<TQuery, TResponse>
- [ ] Queries are read-only operations
- [ ] Proper use of repository methods

---

## Stage 5: Create Services and Validators

**Estimated Time**: 60 minutes  
**Files to Create**: 20 files  
**Status**: PENDING

### Objectives
1. Create Service interfaces
2. Implement Services
3. Create FluentValidation validators
4. Create Application exceptions

### Files to Create

#### Service Interfaces (5 files)
1. `Interfaces/IWorkspaceService.cs`
2. `Interfaces/IProjectService.cs`
3. `Interfaces/ITaskService.cs`
4. `Interfaces/IUserService.cs`
5. `Interfaces/ICommentService.cs`

#### Service Implementations (5 files)
6. `Services/WorkspaceService.cs`
7. `Services/ProjectService.cs`
8. `Services/TaskService.cs`
9. `Services/UserService.cs`
10. `Services/CommentService.cs`

#### Validators (8 files)
11. `Validators/CreateWorkspaceValidator.cs`
12. `Validators/UpdateWorkspaceValidator.cs`
13. `Validators/CreateProjectValidator.cs`
14. `Validators/UpdateProjectValidator.cs`
15. `Validators/CreateTaskValidator.cs`
16. `Validators/UpdateTaskValidator.cs`
17. `Validators/CreateUserValidator.cs`
18. `Validators/CreateCommentValidator.cs`

#### Exceptions (2 files)
19. `Exceptions/ApplicationException.cs`
20. `Exceptions/ValidationException.cs`

### Verification
- [ ] Services use MediatR to send commands/queries
- [ ] Validators properly configured
- [ ] Exceptions properly defined
- [ ] DependencyInjection.cs updated

---

## Stage 6: Update Controllers and Infrastructure

**Estimated Time**: 45 minutes  
**Files to Create**: 10 files  
**Status**: PENDING

### Objectives
1. Update all controllers to use MediatR
2. Separate repository implementations
3. Create infrastructure services
4. Update dependency injection

### Files to Create

#### Infrastructure - Separate Repositories (4 files)
1. `Infrastructure/Repositories/WorkspaceRepository.cs` (separate file)
2. `Infrastructure/Repositories/ProjectRepository.cs` (separate file)
3. `Infrastructure/Repositories/TaskRepository.cs` (separate file)
4. `Infrastructure/Repositories/UserRepository.cs` (separate file)

#### Infrastructure - Services (3 files)
5. `Infrastructure/Services/EmailService.cs`
6. `Infrastructure/Services/FileStorageService.cs`
7. `Infrastructure/Services/CacheService.cs`

#### API - Updated Controllers (5 files)
8. Update `API/Controllers/WorkspacesController.cs` (use MediatR)
9. Update `API/Controllers/ProjectsController.cs` (use MediatR)
10. Update `API/Controllers/TasksController.cs` (use MediatR)
11. Update `API/Controllers/UsersController.cs` (use MediatR)
12. Update `API/Controllers/CommentsController.cs` (use MediatR)

### Verification
- [ ] All controllers use IMediator
- [ ] Repositories in separate files
- [ ] Infrastructure services created
- [ ] DependencyInjection updated
- [ ] Build succeeds
- [ ] All endpoints work

---

## Stage 7: Final Testing and Cleanup

**Estimated Time**: 30 minutes  
**Status**: PENDING

### Objectives
1. Build and test entire solution
2. Remove old files
3. Update documentation
4. Verify all endpoints

### Tasks
- [ ] Run `dotnet build` - verify no errors
- [ ] Delete old files (DTOs.cs, IRepositories.cs, Repositories.cs)
- [ ] Test all API endpoints with Swagger
- [ ] Update IMPLEMENTATION_SUMMARY.md
- [ ] Update README.md
- [ ] Create migration if needed
- [ ] Test database seeding

---

## Execution Order

Execute stages in this order:

1. **Stage 1**: Complete CORE Layer (Foundation)
2. **Stage 2**: Restructure DTOs (Data contracts)
3. **Stage 3**: Commands/Handlers (Write operations)
4. **Stage 4**: Queries/Handlers (Read operations)
5. **Stage 5**: Services/Validators (Business logic)
6. **Stage 6**: Controllers/Infrastructure (API layer)
7. **Stage 7**: Testing/Cleanup (Verification)

## Recovery Instructions

If interrupted at any stage:

1. Check this document for current stage status
2. Review "Progress Tracker" at top
3. Continue from last incomplete stage
4. Each stage is independent and can be completed separately
5. Run `dotnet build` after each stage to verify

## Total Effort

- **Total Files**: ~120 files
- **Estimated Time**: 4-5 hours
- **Stages**: 7 stages
- **Average per Stage**: 35-45 minutes

---

## Current Status

**Last Updated**: 2025-12-02 10:39 UTC  
**Current Stage**: Stage 0 (COMPLETED)  
**Next Stage**: Stage 1 - Complete CORE Layer  
**Overall Progress**: 12/120 files (10%)

---

## Notes

- Each stage builds on the previous one
- Stages can be interrupted and resumed
- Always run `dotnet build` after each stage
- Keep this document updated with progress
- Mark stages as complete with ✅
