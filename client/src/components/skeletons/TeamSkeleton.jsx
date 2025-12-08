export default function TeamSkeleton() {
  return (
    <div className="space-y-6 max-w-6xl mx-auto animate-pulse">
      {/* Header Skeleton */}
      <div className="flex justify-between items-center">
        <div>
          <div className="h-8 bg-gray-200 dark:bg-zinc-700 rounded w-48 mb-2"></div>
          <div className="h-4 bg-gray-100 dark:bg-zinc-800 rounded w-64"></div>
        </div>
        <div className="h-10 bg-gray-200 dark:bg-zinc-700 rounded w-32"></div>
      </div>

      {/* Stats Cards Skeleton */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {[...Array(3)].map((_, i) => (
          <div key={i} className="bg-white dark:bg-zinc-900 rounded-lg p-6 border border-gray-200 dark:border-zinc-800">
            <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-24 mb-2"></div>
            <div className="h-8 bg-gray-300 dark:bg-zinc-600 rounded w-16"></div>
          </div>
        ))}
      </div>

      {/* Team Members Table Skeleton */}
      <div className="bg-white dark:bg-zinc-900 rounded-lg border border-gray-200 dark:border-zinc-800">
        <div className="p-6 border-b border-gray-200 dark:border-zinc-800">
          <div className="h-6 bg-gray-200 dark:bg-zinc-700 rounded w-32"></div>
        </div>
        <div className="divide-y divide-gray-200 dark:divide-zinc-800">
          {[...Array(5)].map((_, i) => (
            <div key={i} className="p-6 flex items-center justify-between">
              <div className="flex items-center gap-4 flex-1">
                <div className="h-10 w-10 bg-gray-200 dark:bg-zinc-700 rounded-full"></div>
                <div className="flex-1">
                  <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-40 mb-2"></div>
                  <div className="h-3 bg-gray-100 dark:bg-zinc-800 rounded w-32"></div>
                </div>
              </div>
              <div className="h-6 bg-gray-200 dark:bg-zinc-700 rounded w-20"></div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
