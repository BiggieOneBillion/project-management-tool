# Quick Start: Frontend-Backend Integration

## ✅ What's Been Set Up

I've created the following files to integrate your frontend with the backend:

### 1. **API Configuration** 
- `src/services/api.js` - Axios instance with interceptors
- `.env.development` - Environment variables

### 2. **API Services**
- `src/services/index.js` - All entity services (workspaces, projects, tasks, users, comments)

---

## 🚀 Quick Start Steps

### Step 1: Install Axios

```bash
npm install axios
```

### Step 2: Start Both Servers

**Terminal 1 - Backend:**
```bash
cd server/src/Project.API
dotnet run
```

**Terminal 2 - Frontend:**
```bash
npm run dev
```

### Step 3: Test the Integration

Open your browser console and test the API:

```javascript
import { workspaceService } from './services';

// Get all workspaces
const workspaces = await workspaceService.getAll();
console.log(workspaces);
```

---

## 📝 Usage Examples

### Example 1: Fetch Workspaces in a Component

```javascript
import { useEffect, useState } from 'react';
import { workspaceService } from '../services';
import toast from 'react-hot-toast';

const MyComponent = () => {
  const [workspaces, setWorkspaces] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await workspaceService.getAll();
        setWorkspaces(response.data);
      } catch (error) {
        toast.error(error.message || 'Failed to load workspaces');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) return <div>Loading...</div>;

  return (
    <div>
      <h2>Workspaces ({workspaces.length})</h2>
      {workspaces.map((workspace) => (
        <div key={workspace.id}>{workspace.name}</div>
      ))}
    </div>
  );
};
```

### Example 2: Create a Workspace

```javascript
import { useState } from 'react';
import { workspaceService } from '../services';
import toast from 'react-hot-toast';

const CreateWorkspaceForm = () => {
  const [formData, setFormData] = useState({
    name: '',
    slug: '',
    description: '',
    ownerId: 'user-1', // Replace with actual user ID
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    try {
      const response = await workspaceService.create(formData);
      toast.success('Workspace created successfully!');
      console.log('Created:', response.data);
    } catch (error) {
      toast.error(error.message || 'Failed to create workspace');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Workspace Name"
        value={formData.name}
        onChange={(e) => setFormData({ ...formData, name: e.target.value })}
        required
      />
      <button type="submit">Create</button>
    </form>
  );
};
```

### Example 3: Update a Project

```javascript
import { projectService } from '../services';
import toast from 'react-hot-toast';

const updateProject = async (projectId) => {
  try {
    const response = await projectService.update(projectId, {
      name: 'Updated Project Name',
      description: 'Updated description',
      priority: 'HIGH',
      status: 'ACTIVE',
    });
    
    toast.success('Project updated!');
    return response.data;
  } catch (error) {
    toast.error(error.message || 'Failed to update project');
  }
};
```

### Example 4: Fetch Tasks for a Project

```javascript
import { taskService } from '../services';

const fetchProjectTasks = async (projectId) => {
  try {
    const response = await taskService.getAll({ projectId });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch tasks:', error);
    return [];
  }
};
```

### Example 5: Bulk Delete Tasks

```javascript
import { taskService } from '../services';
import toast from 'react-hot-toast';

const deleteMultipleTasks = async (taskIds) => {
  try {
    await taskService.bulkDelete(taskIds);
    toast.success(`${taskIds.length} tasks deleted successfully!`);
  } catch (error) {
    toast.error(error.message || 'Failed to delete tasks');
  }
};

// Usage
deleteMultipleTasks(['task-1', 'task-2', 'task-3']);
```

---

## 🔧 API Response Format

All API responses follow this format:

```javascript
{
  success: true,
  data: { /* your data */ },
  message: "Operation successful" // optional
}
```

Error responses:

```javascript
{
  success: false,
  message: "Error message",
  errors: ["Validation error 1", "Validation error 2"] // optional
}
```

---

## 🎯 Available Services

### Workspace Service
```javascript
workspaceService.getAll(userId?)
workspaceService.getById(id, includeMembers?, includeProjects?)
workspaceService.create(data)
workspaceService.update(id, data)
workspaceService.delete(id)
```

### Project Service
```javascript
projectService.getAll(workspaceId?)
projectService.getById(id, includeTasks?, includeMembers?)
projectService.create(data)
projectService.update(id, data)
projectService.delete(id)
```

### Task Service
```javascript
taskService.getAll({ projectId?, userId? })
taskService.getById(id, includeComments?)
taskService.create(data)
taskService.update(id, data)
taskService.delete(id)
taskService.bulkDelete(taskIds)
```

### User Service
```javascript
userService.getAll()
userService.getById(id)
userService.getByEmail(email)
userService.create(data)
userService.update(id, data)
userService.delete(id)
```

### Comment Service
```javascript
commentService.getTaskComments(taskId)
commentService.create(taskId, data)
commentService.update(taskId, commentId, data)
commentService.delete(taskId, commentId)
```

---

## 🐛 Troubleshooting

### CORS Error
**Problem**: `Access to XMLHttpRequest has been blocked by CORS policy`

**Solution**: 
- Ensure backend is running
- Check `Program.cs` has correct CORS policy for `http://localhost:5173`
- Restart backend server

### Network Error
**Problem**: `Network error. Please check your connection.`

**Solution**:
- Verify backend is running at `http://localhost:5000`
- Check `.env.development` has correct API URL
- Test API in Swagger UI: `http://localhost:5000/`

### 401 Unauthorized
**Problem**: Getting 401 errors

**Solution**:
- Authentication not yet implemented
- Remove auth token requirement temporarily
- Or implement JWT authentication

---

## 📚 Next Steps

1. **Install Axios**: `npm install axios`
2. **Test API**: Use browser console to test services
3. **Update Components**: Replace dummy data with API calls
4. **Add Redux**: Implement Redux slices (see FRONTEND_BACKEND_INTEGRATION.md)
5. **Add Error Handling**: Implement error boundaries
6. **Add Loading States**: Show loading indicators

---

## 🎉 You're Ready!

The integration is set up and ready to use. Start by:

1. Running both servers
2. Testing API calls in browser console
3. Updating your components to use the services
4. Replacing dummy data with real API data

For detailed Redux integration, see: `FRONTEND_BACKEND_INTEGRATION.md`
