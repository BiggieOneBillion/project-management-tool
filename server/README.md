# Project Management Backend - ASP.NET Core

A RESTful API backend for the Project Management platform built with **ASP.NET Core 9.0** using **Domain-Driven Design (DDD)** principles and a **4-layer architecture**.

## 🏗️ Architecture

This backend follows a clean, layered architecture with clear separation of concerns:

```
server/
├── src/
│   ├── Project.API/              # Presentation Layer
│   │   ├── Controllers/          # API Controllers
│   │   ├── Program.cs            # Application entry point
│   │   └── appsettings.json      # Configuration
│   ├── Project.APPLICATION/      # Application Layer
│   │   └── (DTOs, Commands, Queries - to be added)
│   ├── Project.CORE/             # Domain Layer
│   │   ├── Entities/             # Domain entities
│   │   ├── ValueObjects/         # Enums and value objects
│   │   └── Interfaces/           # Repository interfaces
│   └── Project.INFRASTRUCTURE/   # Infrastructure Layer
│       ├── Data/                 # DbContext and configurations
│       ├── Repositories/         # Repository implementations
│       └── DependencyInjection.cs
└── ProjectManagement.sln         # Solution file
```

## 🛠️ Tech Stack

- **.NET 9.0** - Latest LTS framework
- **ASP.NET Core 9.0** - Web API framework
- **Entity Framework Core 9.0** - ORM for database access
- **SQL Server** - Database (LocalDB for development)
- **Swagger/OpenAPI** - API documentation
- **MediatR** - CQRS pattern implementation (installed, not yet implemented)
- **FluentValidation** - Input validation (installed, not yet implemented)
- **AutoMapper** - Object mapping (installed, not yet implemented)

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Developer Edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension

## 🚀 Getting Started

### 1. Navigate to Server Directory

```bash
cd server
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Update Database Connection String

Edit `src/Project.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjectManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

> **Note**: Change the connection string if you're not using LocalDB.

### 4. Apply Database Migrations

```bash
dotnet ef database update --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
```

This will create the database and all tables.

### 5. Run the API

```bash
cd src/Project.API
dotnet run
```

Or from the solution root:

```bash
dotnet run --project src/Project.API
```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **Swagger UI**: `http://localhost:5000/swagger`

## 📚 API Endpoints

### Workspaces

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/workspaces` | Get all workspaces |
| GET | `/api/v1/workspaces/{id}` | Get workspace by ID |
| POST | `/api/v1/workspaces` | Create new workspace |
| PUT | `/api/v1/workspaces/{id}` | Update workspace |
| DELETE | `/api/v1/workspaces/{id}` | Delete workspace |

### Example Request (Create Workspace)

```bash
curl -X POST http://localhost:5000/api/v1/workspaces \
  -H "Content-Type: application/json" \
  -d '{
    "name": "My Workspace",
    "slug": "my-workspace",
    "description": "A sample workspace",
    "ownerId": "user_1"
  }'
```

### Example Response

```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "My Workspace",
    "slug": "my-workspace",
    "description": "A sample workspace",
    "ownerId": "user_1",
    "createdAt": "2025-12-02T09:00:00Z",
    "updatedAt": "2025-12-02T09:00:00Z"
  },
  "message": "Workspace created successfully"
}
```

## 🗄️ Database Schema

The database includes the following tables:

- **Users** - User accounts
- **Workspaces** - Organization workspaces
- **WorkspaceMembers** - User membership in workspaces
- **Projects** - Projects within workspaces
- **ProjectMembers** - User membership in projects
- **Tasks** - Tasks within projects
- **Comments** - Comments on tasks

See `/docs/DATABASE_SCHEMA.md` in the root directory for detailed schema information.

## 🔧 Development

### Build the Solution

```bash
dotnet build
```

### Run Tests (when implemented)

```bash
dotnet test
```

### Create a New Migration

```bash
dotnet ef migrations add MigrationName --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
```

### Remove Last Migration

```bash
dotnet ef migrations remove --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
```

### Update Database

```bash
dotnet ef database update --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
```

## 🌐 CORS Configuration

The API is configured to allow requests from the frontend running on `http://localhost:5173`.

To modify CORS settings, edit `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

## 📝 Domain Entities

### Core Entities

- **User**: User accounts with profile information
- **Workspace**: Organization/team workspaces
- **WorkspaceMember**: Junction table for workspace membership with roles
- **ProjectEntity**: Projects with status, priority, and progress tracking
- **ProjectMember**: Junction table for project membership
- **TaskEntity**: Tasks with assignments, due dates, and status
- **Comment**: Comments on tasks

### Value Objects (Enums)

- **Priority**: LOW, MEDIUM, HIGH
- **ProjectStatus**: PLANNING, ACTIVE, ON_HOLD, COMPLETED
- **TaskStatus**: TODO, IN_PROGRESS, DONE, BLOCKED
- **TaskType**: FEATURE, BUG, TASK, IMPROVEMENT, OTHER
- **WorkspaceRole**: ADMIN, MEMBER

## 🔐 Security (To Be Implemented)

The following security features are planned:

- **JWT Authentication**: Token-based authentication
- **ASP.NET Core Identity**: User management
- **Role-Based Authorization**: Workspace and project-level permissions
- **Input Validation**: FluentValidation for all inputs
- **Rate Limiting**: Prevent API abuse

## 📦 Project Structure

### Project.CORE (Domain Layer)
- Contains business logic and domain models
- No dependencies on other layers
- Defines repository interfaces

### Project.INFRASTRUCTURE (Infrastructure Layer)
- Implements data access with EF Core
- Contains DbContext and entity configurations
- Implements repository interfaces

### Project.APPLICATION (Application Layer)
- Orchestrates application flow
- Contains DTOs, Commands, Queries (to be implemented)
- Business logic coordination

### Project.API (Presentation Layer)
- Exposes HTTP endpoints
- Handles HTTP requests/responses
- Swagger documentation

## 🚧 Roadmap

### Completed ✅
- [x] Solution and project structure
- [x] Domain entities and value objects
- [x] Repository pattern implementation
- [x] EF Core DbContext and configurations
- [x] Database migrations
- [x] Workspaces API endpoint
- [x] Swagger documentation
- [x] CORS configuration

### In Progress 🔄
- [ ] Complete CRUD controllers for all entities
- [ ] DTOs and AutoMapper configuration
- [ ] CQRS with MediatR
- [ ] FluentValidation for inputs

### Planned 📋
- [ ] JWT Authentication
- [ ] Authorization policies
- [ ] Seed data for development
- [ ] Unit and integration tests
- [ ] Logging with Serilog
- [ ] Error handling middleware
- [ ] API versioning
- [ ] Caching with Redis
- [ ] Docker support

## 🐛 Troubleshooting

### Migration Issues

If you encounter migration errors:

```bash
# Drop database and recreate
dotnet ef database drop --force --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
dotnet ef database update --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
```

### Connection String Issues

- Ensure SQL Server is running
- Verify connection string in `appsettings.json`
- Check firewall settings
- For LocalDB, ensure it's installed with Visual Studio

### Build Errors

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## 📖 Documentation

For more detailed documentation, see:

- [Backend Architecture](/docs/BACKEND_ARCHITECTURE.md)
- [API Specification](/docs/API_SPECIFICATION.md)
- [Database Schema](/docs/DATABASE_SCHEMA.md)
- [Implementation Guide](/docs/IMPLEMENTATION_GUIDE.md)

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

---

**Built with ❤️ using ASP.NET Core and Domain-Driven Design**
