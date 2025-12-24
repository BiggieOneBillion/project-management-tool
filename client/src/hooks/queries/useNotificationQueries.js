import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { notificationService } from '../../services';
import toast from 'react-hot-toast';

export function useNotifications(onlyUnread = null) {
  return useQuery({
    queryKey: ['notifications', onlyUnread ? 'unread' : 'all'],
    queryFn: () => notificationService.getUserNotifications(onlyUnread),
    refetchInterval: 30000, // Refetch every 30 seconds
  });
}

export function useUnreadCount() {
  return useQuery({
    queryKey: ['notifications', 'unread-count'],
    queryFn: () => notificationService.getUnreadCount(),
    refetchInterval: 30000, // Refetch every 30 seconds
  });
}

export function useMarkAsRead() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (notificationId) => notificationService.markAsRead(notificationId),
    onSuccess: () => {
      queryClient.invalidateQueries(['notifications']);
      queryClient.invalidateQueries(['notifications', 'unread-count']);
    },
  });
}

export function useMarkAllAsRead() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: () => notificationService.markAllAsRead(),
    onSuccess: () => {
      queryClient.invalidateQueries(['notifications']);
      queryClient.invalidateQueries(['notifications', 'unread-count']);
      toast.success('All notifications marked as read');
    },
  });
}
