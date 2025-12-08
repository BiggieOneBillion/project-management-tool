import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { projectService } from '../services';

export const useProjectStore = create(
  persist(
    (set) => ({
  // State
  projects: [],
  currentProject: null,
  loading: false,
  error: null,

  // Actions
  setCurrentProject: (project) => set({ currentProject: project }),

  clearError: () => set({ error: null }),

  // Async actions
  // Async actions removed (replaced by React Query hooks)
  
  // Setters for syncing
  setProjects: (projects) => {
    set({ projects, loading: false });
    
    // Sync currentProject if needed
    const { currentProject } = get();
    if (currentProject) {
        const updated = projects.find(p => p.id === currentProject.id);
        if (updated) {
            set({ currentProject: updated });
        }
    }
  },

  // Sync actions for optimistic updates (if needed)
  addProject: (project) => {
    set((state) => ({
      projects: [...state.projects, project]
    }));
  },

  updateProject: (project) => {
    set((state) => ({
      projects: state.projects.map((p) => p.id === project.id ? project : p),
      currentProject: state.currentProject?.id === project.id ? project : state.currentProject
    }));
  },

  deleteProject: (id) => {
    set((state) => ({
      projects: state.projects.filter((p) => p.id !== id),
      currentProject: state.currentProject?.id === id ? null : state.currentProject
    }));
  },
}),
    {
      name: 'project-storage',
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
