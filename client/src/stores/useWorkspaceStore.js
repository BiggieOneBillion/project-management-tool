import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { workspaceService } from '../services';

// Cache duration in milliseconds (5 minutes)
const CACHE_DURATION = 5 * 60 * 1000;

export const useWorkspaceStore = create(
  persist(
    (set, get) => ({
  // State
  workspaces: [],
  currentWorkspace: null,
  loading: false,
  error: null,
  lastFetched: null,

  // Helper to check if data should be refetched
  shouldFetch: () => {
    const { workspaces, loading, lastFetched } = get();
    if (loading) return false;
    if (workspaces.length === 0) return true;
    if (!lastFetched || Date.now() - lastFetched > CACHE_DURATION) return true;
    return false;
  },

  // Actions
  setCurrentWorkspace: (workspace) => {
    const { workspaces } = get();
    const ws = typeof workspace === 'string' 
      ? workspaces.find((w) => w.id === workspace)
      : workspace;
    
    if (ws) {
      localStorage.setItem('currentWorkspaceId', ws.id);
      set({ currentWorkspace: ws });
    }
  },

  clearError: () => set({ error: null }),

  // Async actions removed (replaced by React Query hooks)
  
  // Setters for syncing with React Query
  setWorkspaces: (workspaces) => {
    set({ workspaces, loading: false, lastFetched: Date.now() });
    
    // Ensure currentWorkspace is valid
    const { currentWorkspace } = get();
    if (currentWorkspace) {
        const updated = workspaces.find(w => w.id === currentWorkspace.id);
        if (updated) {
            set({ currentWorkspace: updated });
        }
    } else if (workspaces.length > 0) {
        // Only set default if none selected
        set({ currentWorkspace: workspaces[0] }); 
        // Actually, we might want to respect localStorage preference if applied elsewhere
    }
  },

  // Removed fetchWorkspaceById, createWorkspace, updateWorkspace, deleteWorkspace
  // These are handled by React Query hooks which call setWorkspaces or invalidate queries.

  // Legacy actions for backward compatibility
  addProject: (project) => {
    set((state) => {
      if (!state.currentWorkspace) return state;
      
      const currentWorkspace = {
        ...state.currentWorkspace,
        projects: [...(state.currentWorkspace.projects || []), project]
      };
      
      return { currentWorkspace };
    });
  },

  addTask: (task) => {
    set((state) => {
      if (!state.currentWorkspace?.projects) return state;
      
      const currentWorkspace = {
        ...state.currentWorkspace,
        projects: state.currentWorkspace.projects.map((p) => {
          if (p.id === task.projectId) {
            return {
              ...p,
              tasks: [...(p.tasks || []), task]
            };
          }
          return p;
        })
      };
      
      return { currentWorkspace };
    });
  },

  updateTask: (task) => {
    set((state) => {
      if (!state.currentWorkspace?.projects) return state;
      
      const currentWorkspace = {
        ...state.currentWorkspace,
        projects: state.currentWorkspace.projects.map((p) => {
          if (p.id === task.projectId) {
            return {
              ...p,
              tasks: p.tasks.map((t) => t.id === task.id ? task : t)
            };
          }
          return p;
        })
      };
      
      return { currentWorkspace };
    });
  },

  deleteTask: (taskIds) => {
    set((state) => {
      if (!state.currentWorkspace?.projects) return state;
      
      const ids = Array.isArray(taskIds) ? taskIds : [taskIds];
      const currentWorkspace = {
        ...state.currentWorkspace,
        projects: state.currentWorkspace.projects.map((p) => ({
          ...p,
          tasks: p.tasks.filter((t) => !ids.includes(t.id))
        }))
      };
      
      return { currentWorkspace };
    });
  },
}),
    {
      name: 'workspace-storage',
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
