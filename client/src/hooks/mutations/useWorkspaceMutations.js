import { useMutation, useQueryClient } from '@tanstack/react-query';
import { workspaceService } from '../../services';
import { useWorkspaceStore } from '../../stores/useWorkspaceStore';
import toast from 'react-hot-toast';

/**
 * Create workspace mutation with optimistic update
 */
export function useCreateWorkspace() {
  const queryClient = useQueryClient();
  const addWorkspace = useWorkspaceStore(state => state.addWorkspace);
  
  return useMutation({
    mutationFn: workspaceService.create,
    onMutate: async (newWorkspace) => {
      // Create temporary ID for optimistic update
      const tempId = `temp-${Date.now()}`;
      const optimisticWorkspace = { 
        ...newWorkspace, 
        id: tempId,
        createdAt: new Date().toISOString(),
      };
      
      // Optimistically add to Zustand
      addWorkspace(optimisticWorkspace);
      
      return { tempId };
    },
    onSuccess: (response, variables, context) => {
      // Replace temp workspace with real one from server
      const updateWorkspace = useWorkspaceStore.getState().updateWorkspace;
      updateWorkspace(context.tempId, response.data);
      
      // Invalidate queries to refetch
      queryClient.invalidateQueries(['workspaces']);
      toast.success('Workspace created successfully');
    },
    onError: (error, variables, context) => {
      // Rollback optimistic update
      const deleteWorkspace = useWorkspaceStore.getState().deleteWorkspace;
      deleteWorkspace(context.tempId);
      toast.error(error?.message || 'Failed to create workspace');
    },
  });
}

/**
 * Update workspace mutation with optimistic update
 */
export function useUpdateWorkspace() {
  const queryClient = useQueryClient();
  const updateWorkspaceInStore = useWorkspaceStore(state => state.updateWorkspace);
  
  return useMutation({
    mutationFn: ({ id, data }) => workspaceService.update(id, data),
    onMutate: async ({ id, data }) => {
      // Cancel outgoing refetches
      await queryClient.cancelQueries(['workspace', id]);
      
      // Snapshot previous value
      const previous = queryClient.getQueryData(['workspace', id]);
      
      // Optimistically update cache
      queryClient.setQueryData(['workspace', id], old => ({ ...old, ...data }));
      
      // Update Zustand
      updateWorkspaceInStore(id, data);
      
      return { previous };
    },
    onSuccess: (response, { id }) => {
      queryClient.invalidateQueries(['workspace', id]);
      queryClient.invalidateQueries(['workspaces']);
      toast.success('Workspace updated successfully');
    },
    onError: (error, { id }, context) => {
      // Rollback
      queryClient.setQueryData(['workspace', id], context.previous);
      toast.error(error?.message || 'Failed to update workspace');
    },
  });
}

/**
 * Delete workspace mutation with optimistic update
 */
export function useDeleteWorkspace() {
  const queryClient = useQueryClient();
  const deleteWorkspaceFromStore = useWorkspaceStore(state => state.deleteWorkspace);
  
  return useMutation({
    mutationFn: workspaceService.delete,
    onMutate: async (workspaceId) => {
      // Optimistically remove from Zustand
      deleteWorkspaceFromStore(workspaceId);
      
      return { workspaceId };
    },
    onSuccess: () => {
      queryClient.invalidateQueries(['workspaces']);
      toast.success('Workspace deleted successfully');
    },
    onError: (error, workspaceId, context) => {
      // Would need to restore - for now just refetch
      queryClient.invalidateQueries(['workspaces']);
      toast.error(error?.message || 'Failed to delete workspace');
    },
  });
}
