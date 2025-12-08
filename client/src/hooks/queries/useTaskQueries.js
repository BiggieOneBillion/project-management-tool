import { useQuery } from '@tanstack/react-query';
import { taskService } from '../../services';
import { useTaskStore } from '../../stores/useTaskStore';

/**
 * Fetch all tasks for a project
 */
export function useProjectTasks(projectId) {
  
  return useQuery({
    queryKey: ['tasks', 'project', projectId],
    queryFn: async () => {
      const response = await taskService.getAll({ projectId });
      return response.data;
    },
    enabled: !!projectId,
  });
}

/**
 * Fetch single task
 */
export function useTask(taskId) {
  const setCurrentTask = useTaskStore(state => state.setCurrentTask);
  
  return useQuery({
    queryKey: ['task', taskId],
    queryFn: async () => {
      const response = await taskService.getById(taskId);
      setCurrentTask(response.data);
      return response.data;
    },
    enabled: !!taskId,
  });
}

/**
 * Fetch user's assigned tasks
 */
export function useUserTasks(userId) {
  return useQuery({
    queryKey: ['tasks', 'user', userId],
    queryFn: async () => {
      const response = await taskService.getAll({ userId });
      return response.data;
    },
    enabled: !!userId,
  });
}

/**
 * Fetch user's assigned tasks by workspace id
 */
export function useUserTasksByWorkspaceId(workspaceId, userId) {
  return useQuery({
    queryKey: ['tasks', 'user', userId, workspaceId],
    queryFn: async () => {
      const response = await taskService.getByWorkspaceId(workspaceId, userId);
      return response.data?.data;
    },
    enabled: !!userId &&  !!workspaceId,
  });
}
