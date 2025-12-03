# Backend Implementation Guide

## Prerequisites

Before starting the backend implementation, ensure you have the following installed:

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (LocalDB, Express, or Developer Edition) - [Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- **Visual Studio 2022** or **Visual Studio Code** with C# extension
- **Postman** or similar API testing tool (optional)
- **Git** for version control

---

## Project Setup

### Step 1: Create Solution and Projects

```bash
# Create solution directory
mkdir ProjectManagement
cd ProjectManagement

# Create solution file
dotnet new sln -n ProjectManagement

# Create projects
dotnet new webapi -n Project.API -o src/Project.API
dotnet new classlib -n Project.CORE -o src/Project.CORE
dotnet new classlib -n Project.APPLICATION -o src/Project.APPLICATION
dotnet new classlib -n Project.INFRASTRUCTURE -o src/Project.INFRASTRUCTURE

# Add projects to solution
dotnet sln add src/Project.API/Project.API.csproj
dotnet sln add src/Project.CORE/Project.CORE.csproj
dotnet sln add src/Project.APPLICATION/Project.APPLICATION.csproj
dotnet sln add src/Project.INFRASTRUCTURE/Project.INFRASTRUCTURE.csproj

# Create test projects
dotnet new xunit -n Project.UnitTests -o tests/Project.UnitTests
dotnet new xunit -n Project.IntegrationTests -o tests/Project.IntegrationTests

dotnet sln add tests/Project.UnitTests/Project.UnitTests.csproj
dotnet sln add tests/Project.IntegrationTests/Project.IntegrationTests.csproj
```

### Step 2: Add Project References

```bash
# API references APPLICATION and INFRASTRUCTURE
cd src/Project.API
dotnet add reference ../Project.APPLICATION/Project.APPLICATION.csproj
dotnet add reference ../Project.INFRASTRUCTURE/Project.INFRASTRUCTURE.csproj

# APPLICATION references CORE
cd ../Project.APPLICATION
dotnet add reference ../Project.CORE/Project.CORE.csproj

# INFRASTRUCTURE references CORE
cd ../Project.INFRASTRUCTURE
dotnet add reference ../Project.CORE/Project.CORE.csproj
```

### Step 3: Install NuGet Packages

#### Project.CORE
```bash
cd src/Project.CORE
# No external dependencies - pure domain logic
```

#### Project.APPLICATION
```bash
cd src/Project.APPLICATION
dotnet add package MediatR
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

#### Project.INFRASTRUCTURE
```bash
cd src/Project.INFRASTRUCTURE
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
```

#### Project.API
```bash
cd src/Project.API
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Cors
dotnet add package AspNetCoreRateLimit
dotnet add package Serilog.AspNetCore
```

#### Test Projects
```bash
cd tests/Project.UnitTests
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory

cd ../Project.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

---

## Implementation Steps

### Phase 1: Domain Layer (Project.CORE)

#### 1.1 Create Entities

Create folder structure:
```
Project.CORE/
├── Entities/
├── ValueObjects/
├── Interfaces/
├── DomainEvents/
├── Exceptions/
└── Specifications/
```

**Example Entity** (`Entities/User.cs`):
```csharp
namespace Project.CORE.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<Workspace> OwnedWorkspaces { get; set; }
        public ICollection<WorkspaceMember> WorkspaceMemberships { get; set; }
        public ICollection<Project> LedProjects { get; set; }
        public ICollection<ProjectMember> ProjectMemberships { get; set; }
        public ICollection<Task> AssignedTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
```

#### 1.2 Create Value Objects

**Example** (`ValueObjects/Priority.cs`):
```csharp
namespace Project.CORE.ValueObjects
{
    public enum Priority
    {
        LOW = 1,
        MEDIUM = 2,
        HIGH = 3
    }
}
```

#### 1.3 Create Repository Interfaces

**Example** (`Interfaces/IRepository.cs`):
```csharp
namespace Project.CORE.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
```

**Example** (`Interfaces/IWorkspaceRepository.cs`):
```csharp
namespace Project.CORE.Interfaces
{
    public interface IWorkspaceRepository : IRepository<Workspace>
    {
        Task<IEnumerable<Workspace>> GetUserWorkspacesAsync(string userId);
        Task<Workspace> GetBySlugAsync(string slug);
    }
}
```

---

### Phase 2: Infrastructure Layer (Project.INFRASTRUCTURE)

#### 2.1 Create DbContext

**File**: `Data/ApplicationDbContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using Project.CORE.Entities;

namespace Project.INFRASTRUCTURE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<WorkspaceMember> WorkspaceMembers { get; set; }
        public DbSet<CORE.Entities.Project> Projects { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<CORE.Entities.Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
```

#### 2.2 Create Entity Configurations

See `DATABASE_SCHEMA.md` for detailed configurations.

#### 2.3 Implement Repositories

**Example** (`Repositories/Repository.cs`):
```csharp
using Microsoft.EntityFrameworkCore;
using Project.CORE.Interfaces;
using Project.INFRASTRUCTURE.Data;

namespace Project.INFRASTRUCTURE.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        
        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        
        public virtual async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
```

#### 2.4 Configure Dependency Injection

**File**: `DependencyInjection.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.CORE.Interfaces;
using Project.INFRASTRUCTURE.Data;
using Project.INFRASTRUCTURE.Repositories;

namespace Project.INFRASTRUCTURE
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            
            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}
```

#### 2.5 Create and Run Migrations

```bash
cd src/Project.INFRASTRUCTURE

# Add migration
dotnet ef migrations add InitialCreate --startup-project ../Project.API

# Update database
dotnet ef database update --startup-project ../Project.API
```

---

### Phase 3: Application Layer (Project.APPLICATION)

#### 3.1 Create DTOs

**Example** (`DTOs/Workspace/WorkspaceDto.cs`):
```csharp
namespace Project.APPLICATION.DTOs.Workspace
{
    public class WorkspaceDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

#### 3.2 Create Commands and Handlers

**Example** (`Commands/Workspace/CreateWorkspaceCommand.cs`):
```csharp
using MediatR;
using Project.APPLICATION.DTOs.Workspace;

namespace Project.APPLICATION.Commands.Workspace
{
    public class CreateWorkspaceCommand : IRequest<WorkspaceDto>
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
    }
}
```

**Handler** (`Commands/Workspace/CreateWorkspaceCommandHandler.cs`):
```csharp
using AutoMapper;
using MediatR;
using Project.CORE.Entities;
using Project.CORE.Interfaces;

namespace Project.APPLICATION.Commands.Workspace
{
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
}
```

#### 3.3 Create Validators

**Example** (`Validators/CreateWorkspaceValidator.cs`):
```csharp
using FluentValidation;
using Project.APPLICATION.Commands.Workspace;

namespace Project.APPLICATION.Validators
{
    public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceCommand>
    {
        public CreateWorkspaceValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
            
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric with hyphens");
            
            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Owner is required");
        }
    }
}
```

#### 3.4 Configure AutoMapper

**File**: `Mappings/MappingProfile.cs`
```csharp
using AutoMapper;
using Project.APPLICATION.DTOs.Workspace;
using Project.CORE.Entities;

namespace Project.APPLICATION.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Workspace, WorkspaceDto>();
            CreateMap<CORE.Entities.Project, ProjectDto>();
            CreateMap<CORE.Entities.Task, TaskDto>();
            // ... more mappings
        }
    }
}
```

#### 3.5 Configure Dependency Injection

**File**: `DependencyInjection.cs`
```csharp
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Project.APPLICATION
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}
```

---

### Phase 4: API Layer (Project.API)

#### 4.1 Configure Program.cs

```csharp
using Project.APPLICATION;
using Project.INFRASTRUCTURE;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

#### 4.2 Create Controllers

**Example** (`Controllers/WorkspacesController.cs`):
```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.APPLICATION.Commands.Workspace;
using Project.APPLICATION.Queries.Workspace;

namespace Project.API.Controllers
{
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
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllWorkspacesQuery();
            var result = await _mediator.Send(query);
            return Ok(new { success = true, data = result });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetWorkspaceByIdQuery { Id = id };
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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateWorkspaceCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(new { success = true, data = result, message = "Workspace updated successfully" });
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteWorkspaceCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
```

#### 4.3 Add Exception Handling Middleware

**File**: `Middleware/ExceptionHandlingMiddleware.cs`
```csharp
using System.Net;
using System.Text.Json;

namespace Project.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }
        
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            var response = new
            {
                success = false,
                message = "An error occurred processing your request",
                error = exception.Message
            };
            
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
```

Register in `Program.cs`:
```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

---

## Running the Application

### Development

```bash
# Navigate to API project
cd src/Project.API

# Run the application
dotnet run

# Or with watch (auto-reload)
dotnet watch run
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5000`
- Swagger: `https://localhost:7001/swagger`

### Testing with Swagger

1. Navigate to `https://localhost:7001/swagger`
2. Test endpoints directly from the Swagger UI
3. View request/response schemas

---

## Testing

### Unit Tests

```bash
cd tests/Project.UnitTests
dotnet test
```

**Example Test**:
```csharp
using Xunit;
using Moq;
using FluentAssertions;
using Project.APPLICATION.Commands.Workspace;
using Project.CORE.Interfaces;

public class CreateWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsWorkspaceDto()
    {
        // Arrange
        var mockRepo = new Mock<IWorkspaceRepository>();
        var handler = new CreateWorkspaceCommandHandler(mockRepo.Object, mapper);
        var command = new CreateWorkspaceCommand
        {
            Name = "Test Workspace",
            Slug = "test-workspace",
            OwnerId = "user_1"
        };
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Workspace");
    }
}
```

### Integration Tests

```bash
cd tests/Project.IntegrationTests
dotnet test
```

---

## Deployment

### Azure App Service

```bash
# Publish the application
dotnet publish -c Release -o ./publish

# Deploy to Azure (using Azure CLI)
az webapp up --name your-app-name --resource-group your-rg
```

### Docker

**Dockerfile**:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Project.API/Project.API.csproj", "Project.API/"]
COPY ["src/Project.APPLICATION/Project.APPLICATION.csproj", "Project.APPLICATION/"]
COPY ["src/Project.CORE/Project.CORE.csproj", "Project.CORE/"]
COPY ["src/Project.INFRASTRUCTURE/Project.INFRASTRUCTURE.csproj", "Project.INFRASTRUCTURE/"]
RUN dotnet restore "Project.API/Project.API.csproj"
COPY . .
WORKDIR "/src/Project.API"
RUN dotnet build "Project.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.API.dll"]
```

Build and run:
```bash
docker build -t project-management-api .
docker run -p 8080:80 project-management-api
```

---

## Environment Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjectManagementDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-here-min-32-chars",
    "Issuer": "ProjectManagementAPI",
    "Audience": "ProjectManagementClient",
    "ExpiryInMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "logs/log-.txt", "rollingInterval": "Day" } }
    ]
  }
}
```

---

## Next Steps

1. **Implement Authentication**: Add JWT authentication with ASP.NET Core Identity
2. **Add Authorization**: Implement role-based and policy-based authorization
3. **Implement Remaining Endpoints**: Complete all CRUD operations
4. **Add Validation**: Ensure all inputs are validated
5. **Write Tests**: Achieve 80%+ code coverage
6. **Add Caching**: Implement Redis caching for frequently accessed data
7. **Add Logging**: Comprehensive logging with Serilog
8. **Performance Optimization**: Add database indexes, query optimization
9. **Security Hardening**: HTTPS, CORS, rate limiting, input sanitization
10. **Documentation**: Complete API documentation with Swagger

---

## Troubleshooting

### Migration Issues
```bash
# Drop database and recreate
dotnet ef database drop --force --startup-project ../Project.API
dotnet ef database update --startup-project ../Project.API
```

### Connection String Issues
- Ensure SQL Server is running
- Verify connection string in `appsettings.json`
- Check firewall settings

### Package Restore Issues
```bash
dotnet restore
dotnet clean
dotnet build
```

---

This guide provides a solid foundation for implementing the backend. Follow the phases sequentially for best results.
