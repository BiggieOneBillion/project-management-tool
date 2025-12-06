import { Navigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../stores/useAuthStore';

export default function ProtectedRoute({ children }) {
  const { isAuthenticated, checkAuth } = useAuthStore();
  const location = useLocation();

  // Check if user is authenticated
  const isAuth = checkAuth();

  if (!isAuth && !isAuthenticated) {
    // Redirect to login with the current location as redirect param
    return <Navigate to={`/login?redirect=${encodeURIComponent(location.pathname)}`} replace />;
  }

  return children;
}
