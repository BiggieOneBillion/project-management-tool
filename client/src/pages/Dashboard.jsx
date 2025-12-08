import {
  BarChart3,
  CheckCircle2,
  Clock,
  ListTodo,
  Plus,
} from "lucide-react";
import { useState, useEffect, use } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import toast from "react-hot-toast";
import StatsGrid from "../components/StatsGrid";
import ProjectOverview from "../components/ProjectOverview";
import RecentActivity from "../components/RecentActivity";
import TasksSummary from "../components/TasksSummary";
import CreateProjectDialog from "../components/CreateProjectDialog";
import CreateWorkspaceModal from "../components/CreateWorkspaceModal";
import { useWorkspaceStore } from "../stores/useWorkspaceStore";
import { useProjectStore } from "../stores/useProjectStore";
import { useTaskStore } from "../stores/useTaskStore";
import { useAuthStore } from "../stores/useAuthStore";
import { useWorkspaces } from "../hooks";
import InvitationBanner from "../components/InvitationBanner";

const Dashboard = () => {
  const {user} = useAuthStore(state => state)
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [isWorkspaceModalOpen, setIsWorkspaceModalOpen] = useState(false);
  const [searchParams, setSearchParams] = useSearchParams();
   const navigate = useNavigate();

  const {loading:workspacesLoading, error, data:workspacess} = useWorkspaces(user?.id);
  const {loading:projectsLoading} = useProjectStore((state) => state);
  const {loading:tasksLoading} = useTaskStore((state) => state);
  const {workspaces} = useWorkspaceStore((state) => state)

  // Check for create=workspace query parameter
  useEffect(() => {
    if (searchParams.get('create') === 'workspace') {
      setIsWorkspaceModalOpen(true);
      // Remove the query parameter
      setSearchParams({});
    }
  }, [searchParams, setSearchParams]);

  if (error) {
    toast.error(error?.message || "Failed to load workspaces");
  }

  const isLoading = workspacesLoading;

  if (isLoading) {
    return (
      <div className="max-w-6xl mx-auto flex items-center justify-center min-h-[400px]">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto mb-4"></div>
          <p className="text-gray-500 dark:text-zinc-400">
            Loading dashboard...
          </p>
        </div>
      </div>
    );
  }

  // No need to check for workspaces here - WorkspaceProtectedRoute handles this

  return (
    <div className="max-w-6xl mx-auto">
      {/* Invitation Banner */}
      <InvitationBanner />

      <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-6 ">
        <div>
          <h1 className="text-xl sm:text-2xl font-semibold text-gray-900 dark:text-white mb-1">
            {" "}
            Welcome back, {user?.name || "User"}{" "}
          </h1>
          <p className="text-gray-500 dark:text-zinc-400 text-sm">
            {" "}
            Here's what's happening with your projects today{" "}
          </p>
        </div>

        <button
          onClick={() => setIsDialogOpen(true)}
          className="flex items-center gap-2 px-5 py-2 text-sm rounded bg-gradient-to-br from-blue-500 to-blue-600 text-white space-x-2 hover:opacity-90 transition"
        >
          <Plus size={16} /> New Project
        </button>

        <CreateProjectDialog
          isDialogOpen={isDialogOpen}
          setIsDialogOpen={setIsDialogOpen}
        />
      </div>

      <StatsGrid />

      <div className="grid lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-8">
          <ProjectOverview />
          <RecentActivity />
        </div>
        <div>
          <TasksSummary />
        </div>
      </div>

      {/* Workspace Modal */}
      <CreateWorkspaceModal 
        isOpen={isWorkspaceModalOpen}
        onClose={() => setIsWorkspaceModalOpen(false)}
      />
    </div>
  );
};

export default Dashboard;
