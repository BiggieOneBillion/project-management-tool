import { useQuery } from '@tanstack/react-query';
import { invitationService } from '../../services';

export function usePendingInvitations(userEmail) {
  return useQuery({
    queryKey: ['invitations', 'pending', userEmail],
    queryFn: async () => {
      const response = await invitationService.getUserInvitations(userEmail);
      return response.data || [];
    },
    enabled: !!userEmail,
  });
}

export function useWorkspaceInvitations(workspaceId) {
  return useQuery({
    queryKey: ['invitations', 'workspace', workspaceId],
    queryFn: async () => {
      const response = await invitationService.getWorkspaceInvitations(workspaceId);
      return response.data;
    },
    enabled: !!workspaceId,
  });
}
