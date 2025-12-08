import { useMutation, useQueryClient } from '@tanstack/react-query';
import { invitationService } from '../../services';
import toast from 'react-hot-toast';

export function useSendInvitation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: invitationService.inviteToWorkspace,
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries(['invitations', 'workspace', variables.workspaceId]);
      toast.success('Invitation sent successfully');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to send invitation');
    },
  });
}

export function useAcceptInvitation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (token) => invitationService.acceptInvitation(token),
    onSuccess: () => {
      // Invalidate both invitations and workspaces queries
      queryClient.invalidateQueries(['invitations', 'pending']);
      queryClient.invalidateQueries(['workspaces']);
      toast.success('Invitation accepted! Welcome to the workspace.');
    },
    onError: (error) => {
      toast.error(error?.response?.data?.message || 'Failed to accept invitation');
    },
  });
}

export function useRejectInvitation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: (invitationId) => invitationService.revokeInvitation(invitationId),
    onSuccess: () => {
      queryClient.invalidateQueries(['invitations', 'pending']);
      toast.success('Invitation rejected');
    },
    onError: (error) => {
      toast.error(error?.response?.data?.message || 'Failed to reject invitation');
    },
  });
}

export function useRevokeInvitation() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: invitationService.revokeInvitation,
    onSuccess: (_, invitationId) => {
      // We might need workspaceId to invalidate specific query, or invalidate all workspace invitations
      queryClient.invalidateQueries(['invitations', 'workspace']);
      toast.success('Invitation revoked successfully');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to revoke invitation');
    },
  });
}
