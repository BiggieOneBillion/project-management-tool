# API Specification

## Base URL
```
Development: https://localhost:7001/api/v1
Production: https://api.projectmanagement.com/api/v1
```

## Authentication

All endpoints (except Auth endpoints) require JWT Bearer token authentication.

**Header**:
```
Authorization: Bearer <jwt_token>
```

---

## Response Format

### Success Response
```json
{
  "success": true,
  "data": { ... },
  "message": "Operation successful",
  "timestamp": "2025-12-01T22:00:00Z"
}
```

### Error Response
```json
{
  "success": false,
  "data": null,
  "message": "Error description",
  "errors": [
    {
      "field": "fieldName",
      "message": "Error message"
    }
  ],
  "timestamp": "2025-12-01T22:00:00Z"
}
```

---

## API Endpoints

### Authentication

#### Register User
```http
POST /api/v1/auth/register
```

**Request Body**:
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "data": {
    "userId": "user_123",
    "email": "john@example.com",
    "name": "John Doe",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "refresh_token_here"
  },
  "message": "User registered successfully"
}
```

#### Login
```http
POST /api/v1/auth/login
```

**Request Body**:
```json
{
  "email": "john@example.com",
  "password": "SecurePassword123!"
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "userId": "user_123",
    "email": "john@example.com",
    "name": "John Doe",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "refresh_token_here",
    "expiresIn": 3600
  },
  "message": "Login successful"
}
```

#### Refresh Token
```http
POST /api/v1/auth/refresh
```

**Request Body**:
```json
{
  "refreshToken": "refresh_token_here"
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "token": "new_jwt_token",
    "refreshToken": "new_refresh_token",
    "expiresIn": 3600
  }
}
```

---

### Workspaces

#### Get All User Workspaces
```http
GET /api/v1/workspaces
```

**Query Parameters**:
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "org_1",
        "name": "Corp Workspace",
        "slug": "corp-workspace",
        "description": null,
        "imageUrl": "https://...",
        "ownerId": "user_3",
        "owner": {
          "id": "user_3",
          "name": "Oliver Watts",
          "email": "oliver@example.com",
          "imageUrl": "https://..."
        },
        "memberCount": 3,
        "projectCount": 2,
        "userRole": "ADMIN",
        "createdAt": "2025-10-13T06:55:44.423Z",
        "updatedAt": "2025-10-13T07:17:36.890Z"
      }
    ],
    "totalCount": 1,
    "page": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

#### Get Workspace by ID
```http
GET /api/v1/workspaces/{workspaceId}
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "id": "org_1",
    "name": "Corp Workspace",
    "slug": "corp-workspace",
    "description": null,
    "imageUrl": "https://...",
    "settings": {},
    "ownerId": "user_3",
    "owner": { ... },
    "members": [
      {
        "id": "member_1",
        "userId": "user_1",
        "workspaceId": "org_1",
        "role": "ADMIN",
        "joinedAt": "2025-10-13T06:55:44.423Z",
        "user": {
          "id": "user_1",
          "name": "Alex Smith",
          "email": "alex@example.com",
          "imageUrl": "https://..."
        }
      }
    ],
    "createdAt": "2025-10-13T06:55:44.423Z",
    "updatedAt": "2025-10-13T07:17:36.890Z"
  }
}
```

#### Create Workspace
```http
POST /api/v1/workspaces
```

**Request Body**:
```json
{
  "name": "My Workspace",
  "slug": "my-workspace",
  "description": "Workspace description"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "data": {
    "id": "org_new",
    "name": "My Workspace",
    "slug": "my-workspace",
    "description": "Workspace description",
    "ownerId": "current_user_id",
    "createdAt": "2025-12-01T22:00:00Z"
  },
  "message": "Workspace created successfully"
}
```

#### Update Workspace
```http
PUT /api/v1/workspaces/{workspaceId}
```

**Request Body**:
```json
{
  "name": "Updated Workspace Name",
  "description": "Updated description",
  "settings": {
    "theme": "dark"
  }
}
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": { ... },
  "message": "Workspace updated successfully"
}
```

#### Delete Workspace
```http
DELETE /api/v1/workspaces/{workspaceId}
```

**Response** (204 No Content)

---

### Workspace Members

#### Get Workspace Members
```http
GET /api/v1/workspaces/{workspaceId}/members
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": [
    {
      "id": "member_1",
      "userId": "user_1",
      "workspaceId": "org_1",
      "role": "ADMIN",
      "joinedAt": "2025-10-13T06:55:44.423Z",
      "user": {
        "id": "user_1",
        "name": "Alex Smith",
        "email": "alex@example.com",
        "imageUrl": "https://..."
      }
    }
  ]
}
```

#### Add Workspace Member
```http
POST /api/v1/workspaces/{workspaceId}/members
```

**Request Body**:
```json
{
  "email": "newmember@example.com",
  "role": "MEMBER",
  "message": "Welcome to the team!"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "data": {
    "id": "member_new",
    "userId": "user_new",
    "workspaceId": "org_1",
    "role": "MEMBER",
    "joinedAt": "2025-12-01T22:00:00Z"
  },
  "message": "Member added successfully"
}
```

#### Update Member Role
```http
PATCH /api/v1/workspaces/{workspaceId}/members/{memberId}
```

**Request Body**:
```json
{
  "role": "ADMIN"
}
```

**Response** (200 OK)

#### Remove Workspace Member
```http
DELETE /api/v1/workspaces/{workspaceId}/members/{memberId}
```

**Response** (204 No Content)

---

### Projects

#### Get Workspace Projects
```http
GET /api/v1/workspaces/{workspaceId}/projects
```

**Query Parameters**:
- `status` (optional): Filter by status (ACTIVE, PLANNING, COMPLETED, ON_HOLD)
- `priority` (optional): Filter by priority (HIGH, MEDIUM, LOW)
- `page` (optional): Page number
- `pageSize` (optional): Items per page

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "project_1",
        "name": "LaunchPad CRM",
        "description": "A next-gen CRM for startups...",
        "priority": "HIGH",
        "status": "ACTIVE",
        "startDate": "2025-10-10T00:00:00.000Z",
        "endDate": "2026-02-28T00:00:00.000Z",
        "teamLeadId": "user_3",
        "teamLead": {
          "id": "user_3",
          "name": "Oliver Watts",
          "email": "oliver@example.com"
        },
        "workspaceId": "org_1",
        "progress": 65,
        "taskCount": 4,
        "memberCount": 3,
        "createdAt": "2025-10-13T08:01:35.491Z",
        "updatedAt": "2025-10-13T08:01:45.620Z"
      }
    ],
    "totalCount": 2,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Project by ID
```http
GET /api/v1/projects/{projectId}
```

**Query Parameters**:
- `includeTasks` (optional): Include tasks (default: false)
- `includeMembers` (optional): Include members (default: false)

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "id": "project_1",
    "name": "LaunchPad CRM",
    "description": "A next-gen CRM for startups...",
    "priority": "HIGH",
    "status": "ACTIVE",
    "startDate": "2025-10-10T00:00:00.000Z",
    "endDate": "2026-02-28T00:00:00.000Z",
    "teamLeadId": "user_3",
    "workspaceId": "org_1",
    "progress": 65,
    "members": [...],
    "tasks": [...],
    "createdAt": "2025-10-13T08:01:35.491Z",
    "updatedAt": "2025-10-13T08:01:45.620Z"
  }
}
```

#### Create Project
```http
POST /api/v1/workspaces/{workspaceId}/projects
```

**Request Body**:
```json
{
  "name": "New Project",
  "description": "Project description",
  "priority": "HIGH",
  "status": "PLANNING",
  "startDate": "2025-12-01T00:00:00.000Z",
  "endDate": "2026-06-01T00:00:00.000Z",
  "teamLeadId": "user_1"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "data": { ... },
  "message": "Project created successfully"
}
```

#### Update Project
```http
PUT /api/v1/projects/{projectId}
```

**Request Body**:
```json
{
  "name": "Updated Project Name",
  "description": "Updated description",
  "priority": "MEDIUM",
  "status": "ACTIVE",
  "progress": 75
}
```

**Response** (200 OK)

#### Delete Project
```http
DELETE /api/v1/projects/{projectId}
```

**Response** (204 No Content)

---

### Project Members

#### Get Project Members
```http
GET /api/v1/projects/{projectId}/members
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": [
    {
      "id": "pm_1",
      "userId": "user_1",
      "projectId": "project_1",
      "addedAt": "2025-10-13T08:01:35.491Z",
      "user": {
        "id": "user_1",
        "name": "Alex Smith",
        "email": "alex@example.com",
        "imageUrl": "https://..."
      }
    }
  ]
}
```

#### Add Project Member
```http
POST /api/v1/projects/{projectId}/members
```

**Request Body**:
```json
{
  "userId": "user_2"
}
```

**Response** (201 Created)

#### Remove Project Member
```http
DELETE /api/v1/projects/{projectId}/members/{memberId}
```

**Response** (204 No Content)

---

### Tasks

#### Get Project Tasks
```http
GET /api/v1/projects/{projectId}/tasks
```

**Query Parameters**:
- `status` (optional): Filter by status (TODO, IN_PROGRESS, DONE)
- `type` (optional): Filter by type (FEATURE, BUG, TASK, IMPROVEMENT, OTHER)
- `priority` (optional): Filter by priority
- `assigneeId` (optional): Filter by assignee
- `page`, `pageSize`: Pagination

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "task_1",
        "projectId": "project_1",
        "title": "Design Dashboard UI",
        "description": "Create a modern, responsive CRM dashboard layout.",
        "status": "IN_PROGRESS",
        "type": "FEATURE",
        "priority": "HIGH",
        "assigneeId": "user_1",
        "assignee": {
          "id": "user_1",
          "name": "Alex Smith",
          "email": "alex@example.com"
        },
        "dueDate": "2025-10-31T00:00:00.000Z",
        "commentCount": 0,
        "createdAt": "2025-10-13T08:04:04.084Z",
        "updatedAt": "2025-10-13T08:04:04.084Z"
      }
    ],
    "totalCount": 4,
    "page": 1,
    "pageSize": 10
  }
}
```

#### Get Task by ID
```http
GET /api/v1/tasks/{taskId}
```

**Query Parameters**:
- `includeComments` (optional): Include comments (default: false)

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "id": "task_1",
    "projectId": "project_1",
    "title": "Design Dashboard UI",
    "description": "Create a modern, responsive CRM dashboard layout.",
    "status": "IN_PROGRESS",
    "type": "FEATURE",
    "priority": "HIGH",
    "assigneeId": "user_1",
    "assignee": { ... },
    "dueDate": "2025-10-31T00:00:00.000Z",
    "comments": [...],
    "createdAt": "2025-10-13T08:04:04.084Z",
    "updatedAt": "2025-10-13T08:04:04.084Z"
  }
}
```

#### Create Task
```http
POST /api/v1/projects/{projectId}/tasks
```

**Request Body**:
```json
{
  "title": "New Task",
  "description": "Task description",
  "type": "FEATURE",
  "priority": "HIGH",
  "status": "TODO",
  "assigneeId": "user_1",
  "dueDate": "2025-12-31T00:00:00.000Z"
}
```

**Response** (201 Created):
```json
{
  "success": true,
  "data": { ... },
  "message": "Task created successfully"
}
```

#### Update Task
```http
PUT /api/v1/tasks/{taskId}
```

**Request Body**:
```json
{
  "title": "Updated Task Title",
  "description": "Updated description",
  "status": "IN_PROGRESS",
  "priority": "MEDIUM",
  "assigneeId": "user_2"
}
```

**Response** (200 OK)

#### Delete Task
```http
DELETE /api/v1/tasks/{taskId}
```

**Response** (204 No Content)

#### Bulk Delete Tasks
```http
POST /api/v1/tasks/bulk-delete
```

**Request Body**:
```json
{
  "taskIds": ["task_1", "task_2", "task_3"]
}
```

**Response** (200 OK)

---

### Comments

#### Get Task Comments
```http
GET /api/v1/tasks/{taskId}/comments
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": [
    {
      "id": "comment_1",
      "taskId": "task_1",
      "userId": "user_1",
      "content": "This is a comment",
      "user": {
        "id": "user_1",
        "name": "Alex Smith",
        "imageUrl": "https://..."
      },
      "createdAt": "2025-10-15T10:30:00.000Z",
      "updatedAt": "2025-10-15T10:30:00.000Z"
    }
  ]
}
```

#### Add Comment
```http
POST /api/v1/tasks/{taskId}/comments
```

**Request Body**:
```json
{
  "content": "This is a new comment"
}
```

**Response** (201 Created)

#### Update Comment
```http
PUT /api/v1/comments/{commentId}
```

**Request Body**:
```json
{
  "content": "Updated comment content"
}
```

**Response** (200 OK)

#### Delete Comment
```http
DELETE /api/v1/comments/{commentId}
```

**Response** (204 No Content)

---

### Users

#### Get Current User
```http
GET /api/v1/users/me
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "id": "user_1",
    "name": "Alex Smith",
    "email": "alex@example.com",
    "imageUrl": "https://...",
    "createdAt": "2025-10-06T11:04:03.485Z",
    "updatedAt": "2025-10-06T11:04:03.485Z"
  }
}
```

#### Update User Profile
```http
PUT /api/v1/users/me
```

**Request Body**:
```json
{
  "name": "Alex Johnson",
  "imageUrl": "https://new-image-url.com/avatar.jpg"
}
```

**Response** (200 OK)

#### Get User by ID
```http
GET /api/v1/users/{userId}
```

**Response** (200 OK)

---

### Analytics

#### Get Workspace Analytics
```http
GET /api/v1/workspaces/{workspaceId}/analytics
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "totalProjects": 5,
    "activeProjects": 3,
    "totalTasks": 25,
    "completedTasks": 15,
    "totalMembers": 8,
    "averageProjectProgress": 65,
    "tasksByStatus": {
      "TODO": 5,
      "IN_PROGRESS": 5,
      "DONE": 15
    },
    "tasksByType": {
      "FEATURE": 10,
      "BUG": 5,
      "TASK": 8,
      "IMPROVEMENT": 2
    },
    "projectsByStatus": {
      "ACTIVE": 3,
      "PLANNING": 1,
      "COMPLETED": 1
    }
  }
}
```

#### Get Project Analytics
```http
GET /api/v1/projects/{projectId}/analytics
```

**Response** (200 OK):
```json
{
  "success": true,
  "data": {
    "totalTasks": 10,
    "completedTasks": 6,
    "progress": 65,
    "tasksByStatus": { ... },
    "tasksByType": { ... },
    "tasksByPriority": { ... },
    "teamProductivity": [
      {
        "userId": "user_1",
        "userName": "Alex Smith",
        "tasksCompleted": 3,
        "tasksInProgress": 2
      }
    ]
  }
}
```

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Bad Request - Invalid input |
| 401 | Unauthorized - Authentication required |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource doesn't exist |
| 409 | Conflict - Resource already exists |
| 422 | Unprocessable Entity - Validation failed |
| 429 | Too Many Requests - Rate limit exceeded |
| 500 | Internal Server Error |
| 503 | Service Unavailable |

---

## Rate Limiting

- **Authenticated users**: 1000 requests per hour
- **Unauthenticated users**: 100 requests per hour

**Headers**:
```
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 999
X-RateLimit-Reset: 1638360000
```

---

## Pagination

All list endpoints support pagination:

**Query Parameters**:
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 10, max: 100)

**Response**:
```json
{
  "items": [...],
  "totalCount": 100,
  "page": 1,
  "pageSize": 10,
  "totalPages": 10
}
```

---

## Filtering & Sorting

**Filtering** (where applicable):
```
GET /api/v1/projects?status=ACTIVE&priority=HIGH
```

**Sorting**:
```
GET /api/v1/tasks?sortBy=dueDate&sortOrder=asc
```

**Search**:
```
GET /api/v1/projects?search=CRM
```

---

## Webhooks (Future Enhancement)

Webhooks for real-time notifications:

**Events**:
- `workspace.created`
- `project.created`
- `project.updated`
- `task.created`
- `task.assigned`
- `task.completed`
- `comment.added`

---

## API Versioning

- Current version: **v1**
- Version in URL: `/api/v1/...`
- Backward compatibility maintained for at least 6 months after new version release
