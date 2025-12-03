import api from './api';

export const workspaceService = { 
  getAll: async (userId = "user_1") => {
    const params = userId ? { userId } : {};
    return await api.get('/workspaces', { params });
  },

  getById: async (id, includeMembers = false, includeProjects = false) => {
    return await api.get(`/workspaces/${id}`, {
      params: { includeMembers, includeProjects },
    });
  },

  create: async (data) => {
    return await api.post('/workspaces', data);
  },

  update: async (id, data) => {
    return await api.put(`/workspaces/${id}`, data);
  },

  delete: async (id) => {
    return await api.delete(`/workspaces/${id}`);
  },
};

export const projectService = {
  getAll: async (workspaceId = null) => {
    const params = workspaceId ? { workspaceId } : {};
    return await api.get('/projects', { params });
  },

  getById: async (id, includeTasks = false, includeMembers = false) => {
    return await api.get(`/projects/${id}`, {
      params: { includeTasks, includeMembers },
    });
  },

  create: async (data) => {
    return await api.post('/projects', data);
  },

  update: async (id, data) => {
    return await api.put(`/projects/${id}`, data);
  },

  delete: async (id) => {
    return await api.delete(`/projects/${id}`);
  },
};

export const taskService = {
  getAll: async (filters = {}) => {
    const { projectId, userId } = filters;
    const params = {};
    if (projectId) params.projectId = projectId;
    if (userId) params.userId = userId;
    return await api.get('/tasks', { params });
  },

  getById: async (id, includeComments = false) => {
    return await api.get(`/tasks/${id}`, {
      params: { includeComments },
    });
  },

  create: async (data) => {
    return await api.post('/tasks', data);
  },

  update: async (id, data) => {
    return await api.put(`/tasks/${id}`, data);
  },

  delete: async (id) => {
    return await api.delete(`/tasks/${id}`);
  },

  bulkDelete: async (taskIds) => {
    return await api.post('/tasks/bulk-delete', taskIds);
  },
};

export const userService = {
  getAll: async () => {
    return await api.get('/users');
  },

  getById: async (id) => {
    return await api.get(`/users/${id}`);
  },

  getByEmail: async (email) => {
    return await api.get(`/users/email/${email}`);
  },

  create: async (data) => {
    return await api.post('/users', data);
  },

  update: async (id, data) => {
    return await api.put(`/users/${id}`, data);
  },

  delete: async (id) => {
    return await api.delete(`/users/${id}`);
  },
};

export const commentService = {
  getTaskComments: async (taskId) => {
    return await api.get(`/tasks/${taskId}/comments`);
  },

  create: async (taskId, data) => {
    return await api.post(`/tasks/${taskId}/comments`, data);
  },

  update: async (taskId, commentId, data) => {
    return await api.put(`/tasks/${taskId}/comments/${commentId}`, data);
  },

  delete: async (taskId, commentId) => {
    return await api.delete(`/tasks/${taskId}/comments/${commentId}`);
  },
};
