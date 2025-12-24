export default function ProjectsSkeleton() {
  return (
    <div className="max-w-6xl mx-auto animate-pulse">
      {/* Header Skeleton */}
      <div className="flex justify-between items-center mb-6">
        <div className="h-8 bg-gray-200 dark:bg-zinc-700 rounded w-40"></div>
        <div className="h-10 bg-gray-200 dark:bg-zinc-700 rounded w-32"></div>
      </div>

      {/* Projects Grid Skeleton */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {[...Array(6)].map((_, i) => (
          <div key={i} className="bg-white dark:bg-zinc-900 rounded-lg p-6 border border-gray-200 dark:border-zinc-800">
            {/* Project Header */}
            <div className="flex items-start justify-between mb-4">
              <div className="flex-1">
                <div className="h-6 bg-gray-200 dark:bg-zinc-700 rounded w-3/4 mb-2"></div>
                <div className="h-4 bg-gray-100 dark:bg-zinc-800 rounded w-1/2"></div>
              </div>
              <div className="h-8 w-8 bg-gray-200 dark:bg-zinc-700 rounded"></div>
            </div>

            {/* Progress Bar */}
            <div className="mb-4">
              <div className="h-2 bg-gray-100 dark:bg-zinc-800 rounded-full overflow-hidden">
                <div className="h-full bg-gray-300 dark:bg-zinc-600 rounded-full w-1/2"></div>
              </div>
            </div>

            {/* Stats */}
            <div className="flex items-center justify-between text-sm">
              <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-20"></div>
              <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-24"></div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
