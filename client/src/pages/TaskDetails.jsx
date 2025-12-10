import { format } from "date-fns";
import { useNavigate, useSearchParams } from "react-router-dom";
import { ArrowLeftIcon, CalendarIcon, PenIcon } from "lucide-react";
import { useTask, useProject, useTaskComments } from "../hooks";
import { useAuthStore } from "../stores/useAuthStore";
import CommentThread from "../components/CommentThread";

const TaskDetails = () => {
  const [searchParams] = useSearchParams();
  const projectId = searchParams.get("projectId");
  const taskId = searchParams.get("taskId");

  const { data: currentTask, isLoading: loadingTask } = useTask(taskId);
  const { data: project, isLoading: loadingProject } = useProject(projectId);
  const { data: comments = [], isLoading: loadingComments } = useTaskComments(taskId);
  const { user } = useAuthStore();
  const navigate = useNavigate();



  if (loadingTask || loadingProject)
    return (
      <div className="text-gray-500 dark:text-zinc-400 px-4 py-6">
        Loading task details...
      </div>
    );
  if (!currentTask)
    return <div className="text-red-500 px-4 py-6">Task not found.</div>;

  return (
    <div className="flex flex-col-reverse lg:flex-row gap-6 sm:p-4 text-gray-900 dark:text-zinc-100 max-w-6xl mx-auto">
      <div>
       <button
        className="p-1 rounded hover:bg-zinc-200 items-start dark:hover:bg-zinc-700 text-zinc-600 dark:text-zinc-400"
        onClick={() => navigate("/projects")}
      >
        <ArrowLeftIcon className="w-4 h-4" />
      </button>
      </div>
      {/* Left: Comments / Chatbox */}
      <div className="w-full lg:w-2/3">
        <CommentThread
          taskId={taskId}
          comments={comments}
          currentUserId={user?.id}
          isLoading={loadingComments}
        />
      </div>

      {/* Right: Task + Project Info */}
      <div className="w-full lg:w-1/2 flex flex-col gap-6">
        {/* Task Info */}
        <div className="p-5 rounded-md bg-white dark:bg-zinc-900 border border-gray-300 dark:border-zinc-800 ">
          <div className="mb-3">
            <h1 className="text-lg font-medium text-gray-900 dark:text-zinc-100">
              {currentTask.title}
            </h1>
            <div className="flex flex-wrap gap-2 mt-2">
              <span className="px-2 py-0.5 rounded bg-zinc-200 dark:bg-zinc-700 text-zinc-900 dark:text-zinc-300 text-xs">
                {currentTask.status}
              </span>
              <span className="px-2 py-0.5 rounded bg-blue-200 dark:bg-blue-900 text-blue-900 dark:text-blue-300 text-xs">
                {currentTask.type}
              </span>
              <span className="px-2 py-0.5 rounded bg-green-200 dark:bg-emerald-900 text-green-900 dark:text-emerald-300 text-xs">
                {currentTask.priority}
              </span>
            </div>
          </div>

          {currentTask.description && (
            <p className="text-sm text-gray-600 dark:text-zinc-400 leading-relaxed mb-4">
              {currentTask.description}
            </p>
          )}

          <hr className="border-zinc-200 dark:border-zinc-700 my-3" />

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 text-sm text-gray-700 dark:text-zinc-300">
            <div className="flex items-center gap-2">
              <img
                src={currentTask.assignee?.image}
                className="size-5 rounded-full"
                alt="avatar"
              />
              {currentTask.assignee?.name || "Unassigned"}
            </div>
            <div className="flex items-center gap-2">
              <CalendarIcon className="size-4 text-gray-500 dark:text-zinc-500" />
              Due : {format(new Date(currentTask.dueDate), "dd MMM yyyy")}
            </div>
          </div>
        </div>

        {/* Project Info */}
        {project && (
          <div className="p-4 rounded-md bg-white dark:bg-zinc-900 text-zinc-700 dark:text-zinc-200 border border-gray-300 dark:border-zinc-800 ">
            <p className="text-xl font-medium mb-4">Project Details</p>
            <h2 className="text-gray-900 dark:text-zinc-100 flex items-center gap-2">
              {" "}
              <PenIcon className="size-4" /> {project.name}
            </h2>
            <p className="text-xs mt-3">
              Project Start Date:{" "}
              {format(new Date(project.startDate), "dd MMM yyyy")}
            </p>
            <div className="flex flex-wrap gap-4 text-sm text-gray-500 dark:text-zinc-400 mt-3">
              <span>Status: {project.status}</span>
              <span>Priority: {project.priority}</span>
              <span>Progress: {project.progress}%</span>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default TaskDetails;
