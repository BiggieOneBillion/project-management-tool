// Placeholder for comment queries
import { useQuery } from '@tanstack/react-query';
import { commentService } from '../../services';

export function useTaskComments(taskId) {
  return useQuery({
    queryKey: ['comments', 'task', taskId],
    queryFn: async () => {
      const response = await commentService.getTaskComments(taskId);
      return response.data;
    },
    enabled: !!taskId,
  });
}
