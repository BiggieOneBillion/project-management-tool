import { create } from 'zustand';
import { workspaceService } from '../services';

// Cache duration in milliseconds (5 minutes)
const CACHE_DURATION = 5 * 60 * 1000;

export const useWorkspaceStore = create((set, get) => ({
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

  // Async actions
  fetchWorkspaces: async (userId = null) => {
    const { loading } = get();
    if (loading) return;

    console.log(123456);

    // set({ loading: true, error: null });
    try {
      const response = await workspaceService.getAll(userId);
      const workspaces = response.data;
      
      // Set current workspace from localStorage or first workspace
      const savedId = localStorage.getItem('currentWorkspaceId');
      let currentWorkspace = null;
      
      if (savedId) {
        currentWorkspace = workspaces.find((w) => w.id === savedId) || workspaces[0];
      } else if (workspaces.length > 0) {
        currentWorkspace = workspaces[0];
      }

      set({ 
        workspaces, 
        currentWorkspace,
        loading: false, 
        lastFetched: Date.now() 
      });
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to fetch workspaces' 
      });
    }
  },

  fetchWorkspaceById: async (id, includeMembers = false, includeProjects = false) => {
    set({ loading: true, error: null });
    try {
      const response = await workspaceService.getById(id, includeMembers, includeProjects);
      const workspace = response.data;
      
      set((state) => {
        // Update in workspaces array
        const workspaces = state.workspaces.map((w) => 
          w.id === workspace.id ? workspace : w
        );
        
        return {
          workspaces,
          currentWorkspace: workspace,
          loading: false
        };
      });
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to fetch workspace' 
      });
    }
  },

  createWorkspace: async (workspaceData) => {
    set({ loading: true, error: null });
    try {
      const response = await workspaceService.create(workspaceData);
      const workspace = response.data;
      
      localStorage.setItem('currentWorkspaceId', workspace.id);
      
      set((state) => ({
        workspaces: [...state.workspaces, workspace],
        currentWorkspace: workspace,
        loading: false
      }));
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to create workspace' 
      });
    }
  },

  updateWorkspace: async (id, data) => {
    try {
      const response = await workspaceService.update(id, data);
      const workspace = response.data;
      
      set((state) => {
        const workspaces = state.workspaces.map((w) => 
          w.id === workspace.id ? workspace : w
        );
        
        const currentWorkspace = state.currentWorkspace?.id === workspace.id 
          ? workspace 
          : state.currentWorkspace;
        
        return { workspaces, currentWorkspace };
      });
    } catch (error) {
      set({ error: error.message || 'Failed to update workspace' });
    }
  },

  deleteWorkspace: async (id) => {
    try {
      await workspaceService.delete(id);
      
      set((state) => {
        const workspaces = state.workspaces.filter((w) => w.id !== id);
        let currentWorkspace = state.currentWorkspace;
        
        if (state.currentWorkspace?.id === id) {
          currentWorkspace = workspaces[0] || null;
          if (currentWorkspace) {
            localStorage.setItem('currentWorkspaceId', currentWorkspace.id);
          } else {
            localStorage.removeItem('currentWorkspaceId');
          }
        }
        
        return { workspaces, currentWorkspace };
      });
    } catch (error) {
      set({ error: error.message || 'Failed to delete workspace' });
    }
  },

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
}));
