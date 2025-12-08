import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { taskService } from '../services';

export const useTaskStore = create(
  persist(
    (set) => ({
  // State
  tasks: [],
  currentTask: null,
  loading: false,
  error: null,

  // Actions
  setCurrentTask: (task) => set({ currentTask: task }),

  clearError: () => set({ error: null }),

  // Async actions
  // Async actions removed (replaced by React Query hooks)
  
  // Setters for syncing
  setTasks: (tasks) => {
    set({ tasks, loading: false });
    
    // Sync currentTask if needed
    const { currentTask } = get();
    if (currentTask) {
        const updated = tasks.find(t => t.id === currentTask.id);
        if (updated) {
            set({ currentTask: updated });
        }
    }
  },

  addTask: (task) => {
    set((state) => ({
      tasks: [...state.tasks, task]
    }));
  },

  updateTask: (task) => {
    set((state) => ({
      tasks: state.tasks.map((t) => t.id === task.id ? task : t),
      currentTask: state.currentTask?.id === task.id ? task : state.currentTask
    }));
  },

  deleteTask: (id) => {
    set((state) => ({
      tasks: state.tasks.filter((t) => t.id !== id),
      currentTask: state.currentTask?.id === id ? null : state.currentTask
    }));
  },

  bulkDeleteTasks: (taskIds) => {
    set((state) => ({
      tasks: state.tasks.filter((t) => !taskIds.includes(t.id))
    }));
  },
}),
    {
      name: 'task-storage',
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
