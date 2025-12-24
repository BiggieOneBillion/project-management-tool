import { useMutation, useQueryClient } from '@tanstack/react-query';
import { projectService } from '../../services';
import { useProjectStore } from '../../stores/useProjectStore';
import toast from 'react-hot-toast';

/**
 * Create project mutation
 */
export function useCreateProject() {
  const queryClient = useQueryClient();
  const addProject = useProjectStore(state => state.addProject);
  
  return useMutation({
    mutationFn: projectService.create,
    onMutate: async (newProject) => {
      const tempId = `temp-${Date.now()}`;
      const optimisticProject = { 
        ...newProject, 
        id: tempId,
        createdAt: new Date().toISOString(),
      };
      
      addProject(optimisticProject);
      return { tempId };
    },
    onSuccess: (response, variables, context) => {
      const updateProject = useProjectStore.getState().updateProject;
      updateProject(context.tempId, response.data);
      
      queryClient.invalidateQueries(['projects']);
      toast.success('Project created successfully');
    },
    onError: (error, variables, context) => {
      const deleteProject = useProjectStore.getState().deleteProject;
      deleteProject(context.tempId);
      toast.error(error?.message || 'Failed to create project');
    },
  });
}

/**
 * Update project mutation with optimistic update
 */
export function useUpdateProject() {
  const queryClient = useQueryClient();
  const updateProjectInStore = useProjectStore(state => state.updateProject);
  
  return useMutation({
    mutationFn: ({ id, data }) => projectService.update(id, data),
    onMutate: async ({ id, data }) => {
      await queryClient.cancelQueries(['project', id]);
      const previous = queryClient.getQueryData(['project', id]);
      
      // Optimistic update
      queryClient.setQueryData(['project', id], old => ({ ...old, ...data }));
      updateProjectInStore(id, data);
      
      return { previous };
    },
    onSuccess: (response, { id, workspaceId }) => {
      queryClient.invalidateQueries(['project', id]);
      queryClient.invalidateQueries(['projects', workspaceId]);
      toast.success('Project updated successfully');
    },
    onError: (error, { id }, context) => {
      queryClient.setQueryData(['project', id], context.previous);
      toast.error(error?.message || 'Failed to update project');
    },
  });
}

/**
 * Delete project mutation
 */
export function useDeleteProject() {
  const queryClient = useQueryClient();
  const deleteProjectFromStore = useProjectStore(state => state.deleteProject);
  
  return useMutation({
    mutationFn: projectService.delete,
    onMutate: async (projectId) => {
      deleteProjectFromStore(projectId);
      return { projectId };
    },
    onSuccess: (_, projectId) => {
      queryClient.invalidateQueries(['projects']);
      toast.success('Project deleted successfully');
    },
    onError: (error) => {
      queryClient.invalidateQueries(['projects']);
      toast.error(error?.message || 'Failed to delete project');
    },
  });
}
