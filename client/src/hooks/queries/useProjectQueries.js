import { useQuery } from '@tanstack/react-query';
import { projectService } from '../../services';
import { useProjectStore } from '../../stores/useProjectStore';

/**
 * Fetch all projects for a workspace
 */
export function useProjects(workspaceId) {
  const setProjects = useProjectStore(state => state.setProjects);
  
  return useQuery({
    queryKey: ['projects', workspaceId],
    queryFn: async () => {
      const response = await projectService.getAll({ workspaceId });
      setProjects(response.data);
      return response.data;
    },
    enabled: !!workspaceId,
  });
}

/**
 * Fetch single project with optional includes
 */
export function useProject(projectId, options = {}) {
  const setCurrentProject = useProjectStore(state => state.setCurrentProject);
  
  return useQuery({
    queryKey: ['project', projectId, options],
    queryFn: async () => {
      const response = await projectService.getById(projectId, options);
      setCurrentProject(response.data);
      return response.data;
    },
    enabled: !!projectId,
  });
}

/**
 * Fetch project members
 */
export function useProjectMembers(projectId) {
  return useQuery({
    queryKey: ['project', projectId, 'members'],
    queryFn: async () => {
      const response = await projectService.getProjectMembers(projectId);
      return response.data;
    },
    enabled: !!projectId,
  });
}
