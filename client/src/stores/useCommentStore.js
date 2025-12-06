import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { commentService } from '../services';

export const useCommentStore = create(
  persist(
    (set, get) => ({
      // State
      comments: [],
      currentTaskComments: [], // Comments for the currently viewed task
      loading: false,
      error: null,

  // Actions
  clearError: () => set({ error: null }),

  // Fetch comments for a specific task
  fetchTaskComments: async (taskId) => {
    set({ loading: true, error: null });
    try {
      const response = await commentService.getTaskComments(taskId);
      set({ 
        currentTaskComments: response.data,
        comments: response.data, // Also update general comments array
        loading: false 
      });
      return response.data;
    } catch (error) {
      set({
        loading: false,
        error: error.response?.data?.message || error.message || 'Failed to fetch comments'
      });
      throw error;
    }
  },

  // Create a new comment
  createComment: async (taskId, commentData) => {
    set({ loading: true, error: null });
    try {
      const response = await commentService.create(taskId, commentData);
      
      // Add the new comment to the current task comments
      set((state) => ({
        currentTaskComments: [...state.currentTaskComments, response.data],
        comments: [...state.comments, response.data],
        loading: false
      }));
      
      return response.data;
    } catch (error) {
      set({
        loading: false,
        error: error.response?.data?.message || error.message || 'Failed to create comment'
      });
      throw error;
    }
  },

  // Update an existing comment
  updateComment: async (taskId, commentId, commentData) => {
    set({ loading: true, error: null });
    try {
      const response = await commentService.update(taskId, commentId, commentData);
      
      // Update the comment in both arrays
      set((state) => ({
        currentTaskComments: state.currentTaskComments.map((comment) =>
          comment.id === commentId ? response.data : comment
        ),
        comments: state.comments.map((comment) =>
          comment.id === commentId ? response.data : comment
        ),
        loading: false
      }));
      
      return response.data;
    } catch (error) {
      set({
        loading: false,
        error: error.response?.data?.message || error.message || 'Failed to update comment'
      });
      throw error;
    }
  },

  // Delete a comment
  deleteComment: async (taskId, commentId) => {
    set({ loading: true, error: null });
    try {
      await commentService.delete(taskId, commentId);
      
      // Remove the comment from both arrays
      set((state) => ({
        currentTaskComments: state.currentTaskComments.filter(
          (comment) => comment.id !== commentId
        ),
        comments: state.comments.filter(
          (comment) => comment.id !== commentId
        ),
        loading: false
      }));
    } catch (error) {
      set({
        loading: false,
        error: error.response?.data?.message || error.message || 'Failed to delete comment'
      });
      throw error;
    }
  },

  // Clear current task comments (useful when navigating away from a task)
  clearCurrentTaskComments: () => set({ currentTaskComments: [] }),

  // Add a comment optimistically (for immediate UI update)
  addCommentOptimistically: (comment) => {
    set((state) => ({
      currentTaskComments: [...state.currentTaskComments, comment],
      comments: [...state.comments, comment]
    }));
  },

  // Remove a comment optimistically (for immediate UI update)
  removeCommentOptimistically: (commentId) => {
    set((state) => ({
      currentTaskComments: state.currentTaskComments.filter(
        (comment) => comment.id !== commentId
      ),
      comments: state.comments.filter(
        (comment) => comment.id !== commentId
      )
    }));
  },
}),
    {
      name: 'comment-storage',
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
