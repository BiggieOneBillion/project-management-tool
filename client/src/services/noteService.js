import api from './api';

// Note Services
export const noteService = {
  getTaskNotes: async (taskId) => {
    return await api.get(`/tasks/${taskId}/notes`);
  },
  
  getNoteById: async (taskId, noteId) => {
    return await api.get(`/tasks/${taskId}/notes/${noteId}`);
  },
  
  create: async (taskId, data) => {
    return await api.post(`/tasks/${taskId}/notes`, data);
  },
  
  update: async (taskId, noteId, data) => {
    return await api.put(`/tasks/${taskId}/notes/${noteId}`, data);
  },
  
  delete: async (taskId, noteId) => {
    return await api.delete(`/tasks/${taskId}/notes/${noteId}`);
  },
  
  uploadAttachment: async (taskId, noteId, file) => {
    const formData = new FormData();
    formData.append('file', file);
    return await api.post(`/tasks/${taskId}/notes/${noteId}/attachments`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });
  },
  
  deleteAttachment: async (taskId, noteId, attachmentId) => {
    return await api.delete(`/tasks/${taskId}/notes/${noteId}/attachments/${attachmentId}`);
  }
};

// Notification Services
export const notificationService = {
  getUserNotifications: async (onlyUnread = null) => {
    const params = onlyUnread !== null ? { onlyUnread } : {};
    return await api.get('/notifications', { params });
  },
  
  getUnreadCount: async () => {
    return await api.get('/notifications/unread-count');
  },
  
  markAsRead: async (notificationId) => {
    return await api.put(`/notifications/${notificationId}/mark-read`);
  },
  
  markAllAsRead: async () => {
    return await api.put('/notifications/mark-all-read');
  }
};
