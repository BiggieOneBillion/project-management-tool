import { useMutation, useQueryClient } from '@tanstack/react-query';
import { taskService } from '../../services';
import { useTaskStore } from '../../stores/useTaskStore';
import toast from 'react-hot-toast';

/**
 * Create task mutation
 */
export function useCreateTask() {
  const queryClient = useQueryClient();
  const addTask = useTaskStore(state => state.addTask);
  
  return useMutation({
    mutationFn: taskService.create,
    onMutate: async (newTask) => {
      const tempId = `temp-${Date.now()}`;
      const optimisticTask = { 
        ...newTask, 
        id: tempId,
        createdAt: new Date().toISOString(),
      };
      
      addTask(optimisticTask);
      return { tempId, projectId: newTask.projectId };
    },
    onSuccess: (response, variables, context) => {
      const updateTask = useTaskStore.getState().updateTask;
      updateTask(context.tempId, response.data);
      
      queryClient.invalidateQueries(['tasks', 'project', context.projectId]);
      queryClient.invalidateQueries(['project', context.projectId]);
      toast.success('Task created successfully');
    },
    onError: (error, variables, context) => {
      const deleteTask = useTaskStore.getState().deleteTask;
      deleteTask(context.tempId);
      toast.error(error?.message || 'Failed to create task');
    },
  });
}

/**
 * Update task mutation
 */
export function useUpdateTask() {
  const queryClient = useQueryClient();
  const updateTaskInStore = useTaskStore(state => state.updateTask);
  
  return useMutation({
    mutationFn: ({ id, data }) => taskService.update(id, data),
    onMutate: async ({ id, data }) => {
      await queryClient.cancelQueries(['task', id]);
      const previous = queryClient.getQueryData(['task', id]);
      
      queryClient.setQueryData(['task', id], old => ({ ...old, ...data }));
      updateTaskInStore(id, data);
      
      return { previous };
    },
    onSuccess: (response, { id, projectId }) => {
      queryClient.invalidateQueries(['task', id]);
      queryClient.invalidateQueries(['tasks', 'project', projectId]);
      toast.success('Task updated successfully');
    },
    onError: (error, { id }, context) => {
      queryClient.setQueryData(['task', id], context.previous);
      toast.error(error?.message || 'Failed to update task');
    },
  });
}

/**
 * Update task status - optimized for instant UI feedback
 */
export function useUpdateTaskStatus() {
  const queryClient = useQueryClient();
  const updateTaskInStore = useTaskStore(state => state.updateTask);
  
  return useMutation({
    mutationFn: ({ id, status }) => taskService.update(id, { Status: status }),
    onMutate: async ({ id, status }) => {
      // Instant UI update
      const previous = queryClient.getQueryData(['task', id]);
      queryClient.setQueryData(['task', id], old => ({ ...old, status }));
      updateTaskInStore(id, { status });
      
      return { previous };
    },
    onSuccess: (_, { projectId }) => {
      if (projectId) {
        queryClient.invalidateQueries(['tasks', 'project', projectId]);
      }
    },
    onError: (error, { id }, context) => {
      queryClient.setQueryData(['task', id], context.previous);
      toast.error('Failed to update task status');
    },
  });
}

/**
 * Delete task mutation
 */
export function useDeleteTask() {
  const queryClient = useQueryClient();
  const deleteTaskFromStore = useTaskStore(state => state.deleteTask);
  
  return useMutation({
    mutationFn: taskService.delete,
    onMutate: async (taskId) => {
      deleteTaskFromStore(taskId);
      return { taskId };
    },
    onSuccess: (_, taskId, context) => {
      queryClient.invalidateQueries(['tasks']);
      toast.success('Task deleted successfully');
    },
    onError: (error) => {
      queryClient.invalidateQueries(['tasks']);
      toast.error(error?.message || 'Failed to delete task');
    },
  });
}
