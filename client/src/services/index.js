import api from './api';

export const workspaceService = { 
  getAll: async (userId) => {
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
  getAll: async (workspaceId = null, includeTasks = true) => {
    const params = {};
    if (includeTasks) params.includeTasks = includeTasks;
    return await api.get(`/projects?workspaceId=${workspaceId}`, { params });
  },

  getById: async (id, includeTasks = false, includeMembers = false) => {
    return await api.get(`/projects/${id}`, {
      params: { includeTasks, includeMembers },
    });
  },

  getProjectMembers: async (projectId) => {
    const response = await api.get(`/projects/${projectId}/members`);
    return response;
  },

  getProjectMembersList: async (projectId) => {
    const response = await api.get(`/projects/${projectId}/members/list`);
    return response;
  },

  getAvailableWorkspaceMembers: async (projectId, workspaceId) => {
    const response = await api.get(`/projects/${projectId}/available-members`, {
      params: { workspaceId }
    });
    return response;
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

  addMemberToProject: async (projectId, email) => {
    return await api.post(`/projects/${projectId}/members`, { email });
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

   getByWorkspaceId: async (workspaceId, userId) => {
    return await api.get(`/tasks/workspace/${workspaceId}?userId=${userId}`);
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

export const authService = {
  login: async (email, password) => {
    return await api.post('/auth/login', { email, password });
  },

  register: async (name, email, password, invitationToken = null) => {
    return await api.post('/auth/register', { 
      name, 
      email, 
      password, 
      invitationToken 
    });
  },

  refreshToken: async (refreshToken) => {
    return await api.post('/auth/refresh', { refreshToken });
  },

  logout: async () => {
    return await api.post('/auth/logout');
  },

  getCurrentUser: async () => {
    return await api.get('/auth/me');
  },
};

export const invitationService = {
  inviteToWorkspace: async (params) => {
    const payload = {
      WorkspaceId: params.workspaceId,
      Email: params.email,
      Role: params.role,
    }

    return await api.post('/invitations/workspace', payload);
  },

  getWorkspaceInvitations: async (workspaceId, status = 'PENDING') => {
    return await api.get(`/invitations/workspace/${workspaceId}`, {
      params: { status },
    });
  },

  inviteToProject: async (projectId, email) => {
    return await api.post('/invitations/project', {
      projectId,
      email,
    });
  },

  getUserInvitations: async (email) => {
    return await api.get('/invitations/pending', {
      params: { email },
    });
  },

  acceptInvitation: async (token) => {
    return await api.post(`/invitations/accept/${token}`);
  },

  revokeInvitation: async (invitationId) => {
    return await api.post(`/invitations/revoke/${invitationId}`);
  },
};
