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
  // Async actions removed (replaced by React Query hooks)
  
  // Setters/Optimistic Actions
  setComments: (comments) => set({ comments, loading: false }),
  setCurrentTaskComments: (comments) => set({ currentTaskComments: comments, loading: false }),
  
  addComment: (comment) => {
    set((state) => ({
      currentTaskComments: [...state.currentTaskComments, comment],
      comments: [...state.comments, comment]
    }));
  },

  updateComment: (updatedComment) => {
    set((state) => ({
      currentTaskComments: state.currentTaskComments.map((c) =>
        c.id === updatedComment.id ? updatedComment : c
      ),
      comments: state.comments.map((c) =>
        c.id === updatedComment.id ? updatedComment : c
      )
    }));
  },

  deleteComment: (commentId) => {
    set((state) => ({
      currentTaskComments: state.currentTaskComments.filter(
        (c) => c.id !== commentId
      ),
      comments: state.comments.filter(
        (c) => c.id !== commentId
      )
    }));
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
