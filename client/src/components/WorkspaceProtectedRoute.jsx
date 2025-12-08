import { Navigate, Outlet } from 'react-router-dom';
import { useAuthStore } from '../stores/useAuthStore';
import { useWorkspaces } from '../hooks';

export default function WorkspaceProtectedRoute() {
  const { user } = useAuthStore();
  
  // Fetch user's workspaces
  const { data: workspaces, isLoading, error } = useWorkspaces(user?.id);

  // Show loading state while checking workspaces
  if (isLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto mb-4"></div>
          <p className="text-gray-500 dark:text-zinc-400">
            Loading...
          </p>
        </div>
      </div>
    );
  }

  // Show error state if workspace fetch fails
  if (error) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <p className="text-red-500 dark:text-red-400 mb-4">
            Failed to load workspaces
          </p>
          <p className="text-gray-500 dark:text-zinc-400 text-sm">
            {error?.message || 'Please try again later'}
          </p>
        </div>
      </div>
    );
  }

  // Redirect to setup if user has no workspaces
  if (!workspaces || workspaces.length === 0) {
    return <Navigate to="/setup" replace />;
  }

  // User has workspaces, allow access to protected routes
  return <Outlet />;
}
