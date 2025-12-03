import { create } from 'zustand';
import { projectService } from '../services';

export const useProjectStore = create((set) => ({
  // State
  projects: [],
  currentProject: null,
  loading: false,
  error: null,

  // Actions
  setCurrentProject: (project) => set({ currentProject: project }),

  clearError: () => set({ error: null }),

  // Async actions
  fetchProjects: async (workspaceId = null) => {
    set({ loading: true, error: null });
    try {
      const response = await projectService.getAll(workspaceId);
      set({ projects: response.data, loading: false });
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to fetch projects' 
      });
    }
  },

  fetchProjectById: async (id, includeTasks = false, includeMembers = false) => {
    set({ loading: true, error: null });
    try {
      const response = await projectService.getById(id, includeTasks, includeMembers);
      set({ currentProject: response.data, loading: false });
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to fetch project' 
      });
    }
  },

  createProject: async (projectData) => {
    set({ loading: true, error: null });
    try {
      const response = await projectService.create(projectData);
      set((state) => ({
        projects: [...state.projects, response.data],
        loading: false
      }));
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to create project' 
      });
    }
  },

  updateProject: async (id, data) => {
    try {
      const response = await projectService.update(id, data);
      const project = response.data;
      
      set((state) => {
        const projects = state.projects.map((p) => 
          p.id === project.id ? project : p
        );
        
        const currentProject = state.currentProject?.id === project.id 
          ? project 
          : state.currentProject;
        
        return { projects, currentProject };
      });
    } catch (error) {
      set({ error: error.message || 'Failed to update project' });
    }
  },

  deleteProject: async (id) => {
    try {
      await projectService.delete(id);
      
      set((state) => {
        const projects = state.projects.filter((p) => p.id !== id);
        const currentProject = state.currentProject?.id === id 
          ? null 
          : state.currentProject;
        
        return { projects, currentProject };
      });
    } catch (error) {
      set({ error: error.message || 'Failed to delete project' });
    }
  },
}));
