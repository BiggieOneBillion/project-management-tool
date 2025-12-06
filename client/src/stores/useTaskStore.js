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
  fetchTasks: async (filters = {}) => {
    set({ loading: true, error: null });
    try {
      const response = await taskService.getAll(filters);
      set({ tasks: response.data, loading: false });
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to fetch tasks' 
      });
    }
  },

  fetchTaskById: async (id, includeComments = false) => {
    set({ loading: true, error: null });
    try {
      const response = await taskService.getById(id, includeComments);
      set({ currentTask: response.data, loading: false });
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to fetch task' 
      });
    }
  },

  createTask: async (taskData) => {
    set({ loading: true, error: null });
    try {
      const response = await taskService.create(taskData);
      set((state) => ({
        tasks: [...state.tasks, response.data],
        loading: false
      }));
    } catch (error) {
      set({ 
        loading: false, 
        error: error.message || 'Failed to create task' 
      });
    }
  },

  updateTask: async (id, data) => {
    try {
      const response = await taskService.update(id, data);
      const task = response.data;

      console.log("UPDATE RESULT", task)
      
      set((state) => {
        const tasks = state.tasks.map((t) => 
          t.id === task.id ? task : t
        );
        
        const currentTask = state.currentTask?.id === task.id 
          ? task 
          : state.currentTask;
        
        return { tasks, currentTask };
      });
    } catch (error) {
      set({ error: error.message || 'Failed to update task' });
    }
  },

  deleteTask: async (id) => {
    try {
      await taskService.delete(id);
      
      set((state) => {
        const tasks = state.tasks.filter((t) => t.id !== id);
        const currentTask = state.currentTask?.id === id 
          ? null 
          : state.currentTask;
        
        return { tasks, currentTask };
      });
    } catch (error) {
      set({ error: error.message || 'Failed to delete task' });
    }
  },

  bulkDeleteTasks: async (taskIds) => {
    try {
      await taskService.bulkDelete(taskIds);
      
      set((state) => ({
        tasks: state.tasks.filter((t) => !taskIds.includes(t.id))
      }));
    } catch (error) {
      set({ error: error.message || 'Failed to delete tasks' });
    }
  },
}),
    {
      name: 'task-storage',
      storage: createJSONStorage(() => sessionStorage),
    }
  )
);
