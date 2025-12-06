import { useQuery } from '@tanstack/react-query';
import { workspaceService } from '../../services';
import { useWorkspaceStore } from '../../stores/useWorkspaceStore';

/**
 * Fetch all workspaces for a user
 * Syncs data to Zustand for persistence
 */
export function useWorkspaces(userId) {
  const setWorkspaces = useWorkspaceStore(state => state.setWorkspaces);
  
  return useQuery({
    queryKey: ['workspaces', userId],
    queryFn: async () => {
      const response = await workspaceService.getAll(userId);
      // Sync to Zustand for local persistence
      setWorkspaces(response.data);
      return response.data;
    },
    enabled: !!userId,
  });
}

/**
 * Fetch single workspace with details
 * Syncs to Zustand as currentWorkspace
 */
export function useWorkspace(workspaceId, options = {}) {
  const setCurrentWorkspace = useWorkspaceStore(state => state.setCurrentWorkspace);
  
  return useQuery({
    queryKey: ['workspace', workspaceId, options],
    queryFn: async () => {
      const response = await workspaceService.getById(workspaceId, options);
      setCurrentWorkspace(response.data);
      return response.data;
    },
    enabled: !!workspaceId,
  });
}

/**
 * Fetch workspace members
 */
export function useWorkspaceMembers(workspaceId) {
  return useQuery({
    queryKey: ['workspace', workspaceId, 'members'],
    queryFn: async () => {
      const response = await workspaceService.getMembers(workspaceId);
      return response.data;
    },
    enabled: !!workspaceId,
  });
}
