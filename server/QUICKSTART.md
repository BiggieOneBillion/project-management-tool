# Quick Start Guide - Backend

## Setup and Run (5 minutes)

### 1. Navigate to Server Directory
```bash
cd server
```

### 2. Update Database
```bash
dotnet ef database update --project src/Project.INFRASTRUCTURE --startup-project src/Project.API
```

### 3. Run the API
```bash
cd src/Project.API
dotnet run
```

### 4. Test the API

Open your browser and go to:
- **Swagger UI**: http://localhost:5000/swagger

Or test with curl:
```bash
# Get all workspaces
curl http://localhost:5000/api/v1/workspaces

# Create a workspace
curl -X POST http://localhost:5000/api/v1/workspaces \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Workspace",
    "slug": "test-workspace",
    "description": "My first workspace",
    "ownerId": "user_1"
  }'
```

## What's Included

✅ **4-Layer DDD Architecture**
- Project.CORE (Domain)
- Project.INFRASTRUCTURE (Data Access)
- Project.APPLICATION (Business Logic)
- Project.API (HTTP Endpoints)

✅ **Database**
- Entity Framework Core 9.0
- SQL Server with migrations
- Complete schema for all entities

✅ **API Endpoints**
- Workspaces CRUD operations
- Swagger documentation
- CORS enabled for frontend

✅ **Domain Entities**
- User, Workspace, Project, Task, Comment
- Repository pattern
- Value objects (enums)

## Next Steps

1. **Create seed data** for testing
2. **Add more controllers** (Projects, Tasks, Users)
3. **Implement authentication** with JWT
4. **Add validation** with FluentValidation
5. **Connect frontend** to backend API

See `server/README.md` for detailed documentation.
