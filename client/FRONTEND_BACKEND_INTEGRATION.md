# Frontend-Backend Integration Guide

## Overview

This guide explains how to integrate the React frontend with the ASP.NET Core backend API using the CQRS architecture.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [API Service Setup](#api-service-setup)
3. [Redux Integration](#redux-integration)
4. [Component Updates](#component-updates)
5. [Error Handling](#error-handling)
6. [Testing the Integration](#testing-the-integration)

---

## Architecture Overview

### Backend (ASP.NET Core)
- **Base URL**: `http://localhost:5000/api/v1`
- **Architecture**: CQRS with MediatR
- **Response Format**: `{ success: boolean, data: any, message?: string }`

### Frontend (React + Redux)
- **State Management**: Redux Toolkit
- **HTTP Client**: Axios (recommended) or Fetch API
- **Base URL**: `http://localhost:5173`

---

## Step 1: API Service Setup

### 1.1 Install Axios

```bash
npm install axios
```

### 1.2 Create API Configuration

Create `src/services/api.js`:

```javascript
import axios from 'axios';

// Base API configuration
const api = axios.create({
  baseURL: 'http://localhost:5000/api/v1',
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000, // 10 seconds
});

// Request interceptor (for adding auth tokens later)
api.interceptors.request.use(
  (config) => {
    // Add auth token if available
    const token = localStorage.getItem('authToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor (for handling errors globally)
api.interceptors.response.use(
  (response) => {
    // Return the data object from our API response format
    return response.data;
  },
  (error) => {
    // Handle errors globally
    if (error.response) {
      // Server responded with error
      const { status, data } = error.response;
      
      if (status === 401) {
        // Unauthorized - redirect to login
        localStorage.removeItem('authToken');
        window.location.href = '/login';
      }
      
      return Promise.reject(data);
    } else if (error.request) {
      // Request made but no response
      return Promise.reject({
        success: false,
        message: 'Network error. Please check your connection.',
      });
    } else {
      // Something else happened
      return Promise.reject({
        success: false,
        message: error.message,
      });
    }
  }
);

export default api;
```

### 1.3 Create API Services

Create `src/services/workspaceService.js`:

```javascript
import api from './api';

export const workspaceService = {
  // Get all workspaces or filter by userId
  getAll: async (userId = null) => {
    const params = userId ? { userId } : {};
    return await api.get('/workspaces', { params });
  },

  // Get workspace by ID
  getById: async (id, includeMembers = false, includeProjects = false) => {
    return await api.get(`/workspaces/${id}`, {
      params: { includeMembers, includeProjects },
    });
  },

  // Create workspace
  create: async (data) => {
    return await api.post('/workspaces', data);
  },

  // Update workspace
  update: async (id, data) => {
    return await api.put(`/workspaces/${id}`, data);
  },

  // Delete workspace
  delete: async (id) => {
    return await api.delete(`/workspaces/${id}`);
  },
};
```

Create `src/services/projectService.js`:

```javascript
import api from './api';

export const projectService = {
  // Get all projects or filter by workspaceId
  getAll: async (workspaceId = null) => {
    const params = workspaceId ? { workspaceId } : {};
    return await api.get('/projects', { params });
  },

  // Get project by ID
  getById: async (id, includeTasks = false, includeMembers = false) => {
    return await api.get(`/projects/${id}`, {
      params: { includeTasks, includeMembers },
    });
  },

  // Create project
  create: async (data) => {
    return await api.post('/projects', data);
  },

  // Update project
  update: async (id, data) => {
    return await api.put(`/projects/${id}`, data);
  },

  // Delete project
  delete: async (id) => {
    return await api.delete(`/projects/${id}`);
  },
};
```

Create `src/services/taskService.js`:

```javascript
import api from './api';

export const taskService = {
  // Get all tasks or filter by projectId/userId
  getAll: async (filters = {}) => {
    const { projectId, userId } = filters;
    const params = {};
    if (projectId) params.projectId = projectId;
    if (userId) params.userId = userId;
    return await api.get('/tasks', { params });
  },

  // Get task by ID
  getById: async (id, includeComments = false) => {
    return await api.get(`/tasks/${id}`, {
      params: { includeComments },
    });
  },

  // Create task
  create: async (data) => {
    return await api.post('/tasks', data);
  },

  // Update task
  update: async (id, data) => {
    return await api.put(`/tasks/${id}`, data);
  },

  // Delete task
  delete: async (id) => {
    return await api.delete(`/tasks/${id}`);
  },

  // Bulk delete tasks
  bulkDelete: async (taskIds) => {
    return await api.post('/tasks/bulk-delete', taskIds);
  },
};
```

Create `src/services/userService.js`:

```javascript
import api from './api';

export const userService = {
  // Get all users
  getAll: async () => {
    return await api.get('/users');
  },

  // Get user by ID
  getById: async (id) => {
    return await api.get(`/users/${id}`);
  },

  // Get user by email
  getByEmail: async (email) => {
    return await api.get(`/users/email/${email}`);
  },

  // Create user
  create: async (data) => {
    return await api.post('/users', data);
  },

  // Update user
  update: async (id, data) => {
    return await api.put(`/users/${id}`, data);
  },

  // Delete user
  delete: async (id) => {
    return await api.delete(`/users/${id}`);
  },
};
```

Create `src/services/commentService.js`:

```javascript
import api from './api';

export const commentService = {
  // Get comments for a task
  getTaskComments: async (taskId) => {
    return await api.get(`/tasks/${taskId}/comments`);
  },

  // Create comment
  create: async (taskId, data) => {
    return await api.post(`/tasks/${taskId}/comments`, data);
  },

  // Update comment
  update: async (taskId, commentId, data) => {
    return await api.put(`/tasks/${taskId}/comments/${commentId}`, data);
  },

  // Delete comment
  delete: async (taskId, commentId) => {
    return await api.delete(`/tasks/${taskId}/comments/${commentId}`);
  },
};
```

Create `src/services/index.js`:

```javascript
export { workspaceService } from './workspaceService';
export { projectService } from './projectService';
export { taskService } from './taskService';
export { userService } from './userService';
export { commentService } from './commentService';
```

---

## Step 2: Redux Integration

### 2.1 Update Workspace Slice

Update `src/features/workspaceSlice.js`:

```javascript
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { workspaceService } from '../services';

// Async thunks
export const fetchWorkspaces = createAsyncThunk(
  'workspace/fetchAll',
  async (userId = null, { rejectWithValue }) => {
    try {
      const response = await workspaceService.getAll(userId);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to fetch workspaces');
    }
  }
);

export const fetchWorkspaceById = createAsyncThunk(
  'workspace/fetchById',
  async ({ id, includeMembers, includeProjects }, { rejectWithValue }) => {
    try {
      const response = await workspaceService.getById(id, includeMembers, includeProjects);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to fetch workspace');
    }
  }
);

export const createWorkspace = createAsyncThunk(
  'workspace/create',
  async (workspaceData, { rejectWithValue }) => {
    try {
      const response = await workspaceService.create(workspaceData);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to create workspace');
    }
  }
);

export const updateWorkspace = createAsyncThunk(
  'workspace/update',
  async ({ id, data }, { rejectWithValue }) => {
    try {
      const response = await workspaceService.update(id, data);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to update workspace');
    }
  }
);

export const deleteWorkspace = createAsyncThunk(
  'workspace/delete',
  async (id, { rejectWithValue }) => {
    try {
      await workspaceService.delete(id);
      return id;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to delete workspace');
    }
  }
);

// Slice
const workspaceSlice = createSlice({
  name: 'workspace',
  initialState: {
    workspaces: [],
    currentWorkspace: null,
    loading: false,
    error: null,
  },
  reducers: {
    setCurrentWorkspace: (state, action) => {
      state.currentWorkspace = action.payload;
    },
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // Fetch all workspaces
      .addCase(fetchWorkspaces.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchWorkspaces.fulfilled, (state, action) => {
        state.loading = false;
        state.workspaces = action.payload;
      })
      .addCase(fetchWorkspaces.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      // Fetch workspace by ID
      .addCase(fetchWorkspaceById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchWorkspaceById.fulfilled, (state, action) => {
        state.loading = false;
        state.currentWorkspace = action.payload;
      })
      .addCase(fetchWorkspaceById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      // Create workspace
      .addCase(createWorkspace.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(createWorkspace.fulfilled, (state, action) => {
        state.loading = false;
        state.workspaces.push(action.payload);
      })
      .addCase(createWorkspace.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      // Update workspace
      .addCase(updateWorkspace.fulfilled, (state, action) => {
        const index = state.workspaces.findIndex((w) => w.id === action.payload.id);
        if (index !== -1) {
          state.workspaces[index] = action.payload;
        }
        if (state.currentWorkspace?.id === action.payload.id) {
          state.currentWorkspace = action.payload;
        }
      })
      // Delete workspace
      .addCase(deleteWorkspace.fulfilled, (state, action) => {
        state.workspaces = state.workspaces.filter((w) => w.id !== action.payload);
        if (state.currentWorkspace?.id === action.payload) {
          state.currentWorkspace = null;
        }
      });
  },
});

export const { setCurrentWorkspace, clearError } = workspaceSlice.actions;
export default workspaceSlice.reducer;
```

### 2.2 Create Project Slice

Create `src/features/projectSlice.js`:

```javascript
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { projectService } from '../services';

// Async thunks
export const fetchProjects = createAsyncThunk(
  'project/fetchAll',
  async (workspaceId = null, { rejectWithValue }) => {
    try {
      const response = await projectService.getAll(workspaceId);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to fetch projects');
    }
  }
);

export const fetchProjectById = createAsyncThunk(
  'project/fetchById',
  async ({ id, includeTasks, includeMembers }, { rejectWithValue }) => {
    try {
      const response = await projectService.getById(id, includeTasks, includeMembers);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to fetch project');
    }
  }
);

export const createProject = createAsyncThunk(
  'project/create',
  async (projectData, { rejectWithValue }) => {
    try {
      const response = await projectService.create(projectData);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to create project');
    }
  }
);

export const updateProject = createAsyncThunk(
  'project/update',
  async ({ id, data }, { rejectWithValue }) => {
    try {
      const response = await projectService.update(id, data);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to update project');
    }
  }
);

export const deleteProject = createAsyncThunk(
  'project/delete',
  async (id, { rejectWithValue }) => {
    try {
      await projectService.delete(id);
      return id;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to delete project');
    }
  }
);

// Slice
const projectSlice = createSlice({
  name: 'project',
  initialState: {
    projects: [],
    currentProject: null,
    loading: false,
    error: null,
  },
  reducers: {
    setCurrentProject: (state, action) => {
      state.currentProject = action.payload;
    },
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchProjects.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchProjects.fulfilled, (state, action) => {
        state.loading = false;
        state.projects = action.payload;
      })
      .addCase(fetchProjects.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(fetchProjectById.fulfilled, (state, action) => {
        state.currentProject = action.payload;
      })
      .addCase(createProject.fulfilled, (state, action) => {
        state.projects.push(action.payload);
      })
      .addCase(updateProject.fulfilled, (state, action) => {
        const index = state.projects.findIndex((p) => p.id === action.payload.id);
        if (index !== -1) {
          state.projects[index] = action.payload;
        }
      })
      .addCase(deleteProject.fulfilled, (state, action) => {
        state.projects = state.projects.filter((p) => p.id !== action.payload);
      });
  },
});

export const { setCurrentProject, clearError } = projectSlice.actions;
export default projectSlice.reducer;
```

### 2.3 Create Task Slice

Create `src/features/taskSlice.js`:

```javascript
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { taskService } from '../services';

// Async thunks
export const fetchTasks = createAsyncThunk(
  'task/fetchAll',
  async (filters = {}, { rejectWithValue }) => {
    try {
      const response = await taskService.getAll(filters);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to fetch tasks');
    }
  }
);

export const fetchTaskById = createAsyncThunk(
  'task/fetchById',
  async ({ id, includeComments }, { rejectWithValue }) => {
    try {
      const response = await taskService.getById(id, includeComments);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to fetch task');
    }
  }
);

export const createTask = createAsyncThunk(
  'task/create',
  async (taskData, { rejectWithValue }) => {
    try {
      const response = await taskService.create(taskData);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to create task');
    }
  }
);

export const updateTask = createAsyncThunk(
  'task/update',
  async ({ id, data }, { rejectWithValue }) => {
    try {
      const response = await taskService.update(id, data);
      return response.data;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to update task');
    }
  }
);

export const deleteTask = createAsyncThunk(
  'task/delete',
  async (id, { rejectWithValue }) => {
    try {
      await taskService.delete(id);
      return id;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to delete task');
    }
  }
);

export const bulkDeleteTasks = createAsyncThunk(
  'task/bulkDelete',
  async (taskIds, { rejectWithValue }) => {
    try {
      await taskService.bulkDelete(taskIds);
      return taskIds;
    } catch (error) {
      return rejectWithValue(error.message || 'Failed to delete tasks');
    }
  }
);

// Slice
const taskSlice = createSlice({
  name: 'task',
  initialState: {
    tasks: [],
    currentTask: null,
    loading: false,
    error: null,
  },
  reducers: {
    setCurrentTask: (state, action) => {
      state.currentTask = action.payload;
    },
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchTasks.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchTasks.fulfilled, (state, action) => {
        state.loading = false;
        state.tasks = action.payload;
      })
      .addCase(fetchTasks.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(fetchTaskById.fulfilled, (state, action) => {
        state.currentTask = action.payload;
      })
      .addCase(createTask.fulfilled, (state, action) => {
        state.tasks.push(action.payload);
      })
      .addCase(updateTask.fulfilled, (state, action) => {
        const index = state.tasks.findIndex((t) => t.id === action.payload.id);
        if (index !== -1) {
          state.tasks[index] = action.payload;
        }
      })
      .addCase(deleteTask.fulfilled, (state, action) => {
        state.tasks = state.tasks.filter((t) => t.id !== action.payload);
      })
      .addCase(bulkDeleteTasks.fulfilled, (state, action) => {
        state.tasks = state.tasks.filter((t) => !action.payload.includes(t.id));
      });
  },
});

export const { setCurrentTask, clearError } = taskSlice.actions;
export default taskSlice.reducer;
```

### 2.4 Update Store Configuration

Update `src/app/store.js`:

```javascript
import { configureStore } from '@reduxjs/toolkit';
import themeReducer from '../features/themeSlice';
import workspaceReducer from '../features/workspaceSlice';
import projectReducer from '../features/projectSlice';
import taskReducer from '../features/taskSlice';

export const store = configureStore({
  reducer: {
    theme: themeReducer,
    workspace: workspaceReducer,
    project: projectReducer,
    task: taskReducer,
  },
});
```

---

## Step 3: Component Updates

### 3.1 Update Dashboard Component

Update `src/pages/Dashboard.jsx`:

```javascript
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fetchWorkspaces } from '../features/workspaceSlice';
import { fetchProjects } from '../features/projectSlice';
import { fetchTasks } from '../features/taskSlice';
import toast from 'react-hot-toast';

const Dashboard = () => {
  const dispatch = useDispatch();
  const { workspaces, loading: workspacesLoading, error: workspacesError } = useSelector((state) => state.workspace);
  const { projects, loading: projectsLoading } = useSelector((state) => state.project);
  const { tasks, loading: tasksLoading } = useSelector((state) => state.task);

  useEffect(() => {
    // Fetch data on component mount
    const loadData = async () => {
      try {
        await Promise.all([
          dispatch(fetchWorkspaces()).unwrap(),
          dispatch(fetchProjects()).unwrap(),
          dispatch(fetchTasks()).unwrap(),
        ]);
      } catch (error) {
        toast.error(error || 'Failed to load dashboard data');
      }
    };

    loadData();
  }, [dispatch]);

  if (workspacesLoading || projectsLoading || tasksLoading) {
    return <div className="loading">Loading dashboard...</div>;
  }

  return (
    <div className="dashboard">
      <h1>Dashboard</h1>
      
      {/* Workspaces Section */}
      <section>
        <h2>Workspaces ({workspaces.length})</h2>
        {/* Render workspaces */}
      </section>

      {/* Projects Section */}
      <section>
        <h2>Projects ({projects.length})</h2>
        {/* Render projects */}
      </section>

      {/* Tasks Section */}
      <section>
        <h2>Tasks ({tasks.length})</h2>
        {/* Render tasks */}
      </section>
    </div>
  );
};

export default Dashboard;
```

### 3.2 Create Workspace Dialog Component

Update `src/components/CreateWorkspaceDialog.jsx` (if it doesn't exist, create it):

```javascript
import { useState } from 'react';
import { useDispatch } from 'react-redux';
import { createWorkspace } from '../features/workspaceSlice';
import toast from 'react-hot-toast';

const CreateWorkspaceDialog = ({ isOpen, onClose }) => {
  const dispatch = useDispatch();
  const [formData, setFormData] = useState({
    name: '',
    slug: '',
    description: '',
    ownerId: 'user-1', // Replace with actual user ID from auth
  });
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      await dispatch(createWorkspace(formData)).unwrap();
      toast.success('Workspace created successfully!');
      onClose();
      setFormData({ name: '', slug: '', description: '', ownerId: 'user-1' });
    } catch (error) {
      toast.error(error || 'Failed to create workspace');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
      // Auto-generate slug from name
      ...(name === 'name' && { slug: value.toLowerCase().replace(/\s+/g, '-') }),
    }));
  };

  if (!isOpen) return null;

  return (
    <div className="dialog-overlay">
      <div className="dialog">
        <h2>Create Workspace</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Name *</label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
              maxLength={200}
            />
          </div>

          <div className="form-group">
            <label>Slug *</label>
            <input
              type="text"
              name="slug"
              value={formData.slug}
              onChange={handleChange}
              required
              pattern="^[a-z0-9-]+$"
              title="Lowercase letters, numbers, and hyphens only"
              maxLength={200}
            />
          </div>

          <div className="form-group">
            <label>Description</label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              maxLength={1000}
              rows={4}
            />
          </div>

          <div className="dialog-actions">
            <button type="button" onClick={onClose} disabled={loading}>
              Cancel
            </button>
            <button type="submit" disabled={loading}>
              {loading ? 'Creating...' : 'Create Workspace'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateWorkspaceDialog;
```

---

## Step 4: Error Handling

### 4.1 Create Error Boundary Component

Create `src/components/ErrorBoundary.jsx`:

```javascript
import React from 'react';

class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error('Error caught by boundary:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="error-boundary">
          <h1>Something went wrong</h1>
          <p>{this.state.error?.message}</p>
          <button onClick={() => window.location.reload()}>Reload Page</button>
        </div>
      );
    }

    return this.props.children;
  }
}

export default ErrorBoundary;
```

### 4.2 Update App.jsx

```javascript
import { Routes, Route } from 'react-router-dom';
import { Provider } from 'react-redux';
import { store } from './app/store';
import Layout from './pages/Layout';
import { Toaster } from 'react-hot-toast';
import ErrorBoundary from './components/ErrorBoundary';
import Dashboard from './pages/Dashboard';
import Projects from './pages/Projects';
import Team from './pages/Team';
import ProjectDetails from './pages/ProjectDetails';
import TaskDetails from './pages/TaskDetails';

const App = () => {
  return (
    <Provider store={store}>
      <ErrorBoundary>
        <Toaster position="top-right" />
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<Dashboard />} />
            <Route path="team" element={<Team />} />
            <Route path="projects" element={<Projects />} />
            <Route path="projectsDetail" element={<ProjectDetails />} />
            <Route path="taskDetails" element={<TaskDetails />} />
          </Route>
        </Routes>
      </ErrorBoundary>
    </Provider>
  );
};

export default App;
```

---

## Step 5: Testing the Integration

### 5.1 Start Both Servers

**Terminal 1 - Backend:**
```bash
cd server/src/Project.API
dotnet run
```

**Terminal 2 - Frontend:**
```bash
npm run dev
```

### 5.2 Test Checklist

- [ ] Backend running at `http://localhost:5000`
- [ ] Frontend running at `http://localhost:5173`
- [ ] Swagger UI accessible at `http://localhost:5000/`
- [ ] CORS working (no CORS errors in browser console)
- [ ] Data loads in Dashboard
- [ ] Can create new workspace
- [ ] Can create new project
- [ ] Can create new task
- [ ] Error messages display correctly

### 5.3 Common Issues

**CORS Error:**
- Ensure backend `Program.cs` has correct CORS policy
- Check frontend is using `http://localhost:5173`

**Network Error:**
- Verify backend is running
- Check API base URL in `api.js`

**Data Not Loading:**
- Check browser console for errors
- Verify API endpoints in Swagger
- Check Redux DevTools for state

---

## Step 6: Environment Configuration

### 6.1 Create Environment Files

Create `.env.development`:
```
VITE_API_BASE_URL=http://localhost:5000/api/v1
```

Create `.env.production`:
```
VITE_API_BASE_URL=https://your-production-api.com/api/v1
```

### 6.2 Update API Configuration

Update `src/services/api.js`:

```javascript
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api/v1',
  // ... rest of config
});
```

---

## Next Steps

1. **Add Authentication**
   - Implement JWT authentication
   - Add login/register pages
   - Protect routes

2. **Add Loading States**
   - Skeleton loaders
   - Progress indicators

3. **Add Optimistic Updates**
   - Update UI before API response
   - Rollback on error

4. **Add Caching**
   - RTK Query for better caching
   - React Query as alternative

5. **Add Real-time Updates**
   - SignalR for live updates
   - WebSocket integration

---

## Summary

You now have a complete integration between your React frontend and ASP.NET Core backend with:

✅ Axios API client with interceptors
✅ Service layer for all entities
✅ Redux Toolkit with async thunks
✅ Error handling and boundaries
✅ Environment configuration
✅ CORS properly configured

The frontend can now communicate with your CQRS backend seamlessly!
