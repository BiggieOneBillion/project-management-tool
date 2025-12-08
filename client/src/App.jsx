import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { QueryProvider } from './providers/QueryProvider';
import Layout from './pages/Layout';
import ProtectedRoute from './components/ProtectedRoute';
import WorkspaceProtectedRoute from './components/WorkspaceProtectedRoute';
import Dashboard from './pages/Dashboard';
import Projects from './pages/Projects';
import Team from './pages/Team';
import ProjectDetails from './pages/ProjectDetails';
import TaskDetails from './pages/TaskDetails';
import Login from './pages/Login';
import Register from './pages/Register';
import Setup from './pages/Setup';

function App() {
  return (
    <QueryProvider>
      <Router>
        <Toaster position="top-right" />
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />

          {/* Protected Setup Route - Requires authentication but not workspace */}
          <Route
            path="/setup"
            element={
              <ProtectedRoute>
                <Setup />
              </ProtectedRoute>
            }
          />

          {/* Workspace Protected Routes - Requires authentication AND workspace */}
          <Route
            element={
              <ProtectedRoute>
                <WorkspaceProtectedRoute />
              </ProtectedRoute>
            }
          >
            <Route path="/" element={<Layout />}>
              <Route index element={<Dashboard />} />
              <Route path="team" element={<Team />} />
              <Route path="projects" element={<Projects />} />
              <Route path="projectsDetail" element={<ProjectDetails />} />
              <Route path="taskDetails" element={<TaskDetails />} />
            </Route>
          </Route>
        </Routes>
      </Router>
    </QueryProvider>
  );
}

export default App;
