// Placeholder for comment mutations
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { commentService } from '../../services';
import toast from 'react-hot-toast';

export function useCreateComment() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, data }) => commentService.create(taskId, data),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries(['comments', 'task', taskId]);
      toast.success('Comment added');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to add comment');
    },
  });
}

export function useUpdateComment() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, commentId, data }) => commentService.update(taskId, commentId, data),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries(['comments', 'task', taskId]);
      toast.success('Comment updated');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to update comment');
    },
  });
}

export function useDeleteComment() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, commentId }) => commentService.delete(taskId, commentId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries(['comments', 'task', taskId]);
      toast.success('Comment deleted');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to delete comment');
    },
  });
}
