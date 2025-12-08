export default function DashboardSkeleton() {
  return (
    <div className="max-w-6xl mx-auto animate-pulse">
      {/* Stats Grid Skeleton */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 my-9">
        {[...Array(4)].map((_, i) => (
          <div key={i} className="bg-white dark:bg-zinc-900 rounded-lg p-6 border border-gray-200 dark:border-zinc-800">
            <div className="flex items-center justify-between">
              <div className="flex-1">
                <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-20 mb-2"></div>
                <div className="h-8 bg-gray-300 dark:bg-zinc-600 rounded w-16"></div>
              </div>
              <div className="h-12 w-12 bg-gray-200 dark:bg-zinc-700 rounded-lg"></div>
            </div>
          </div>
        ))}
      </div>

      {/* Recent Activity Skeleton */}
      <div className="bg-white dark:bg-zinc-900 rounded-lg p-6 border border-gray-200 dark:border-zinc-800 mb-6">
        <div className="h-6 bg-gray-200 dark:bg-zinc-700 rounded w-40 mb-4"></div>
        <div className="space-y-3">
          {[...Array(5)].map((_, i) => (
            <div key={i} className="flex items-center gap-3">
              <div className="h-10 w-10 bg-gray-200 dark:bg-zinc-700 rounded-full"></div>
              <div className="flex-1">
                <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-3/4 mb-2"></div>
                <div className="h-3 bg-gray-100 dark:bg-zinc-800 rounded w-1/2"></div>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Tasks Summary Skeleton */}
      <div className="bg-white dark:bg-zinc-900 rounded-lg p-6 border border-gray-200 dark:border-zinc-800">
        <div className="h-6 bg-gray-200 dark:bg-zinc-700 rounded w-32 mb-4"></div>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {[...Array(3)].map((_, i) => (
            <div key={i} className="p-4 bg-gray-50 dark:bg-zinc-800 rounded-lg">
              <div className="h-4 bg-gray-200 dark:bg-zinc-700 rounded w-20 mb-3"></div>
              <div className="space-y-2">
                {[...Array(3)].map((_, j) => (
                  <div key={j} className="h-3 bg-gray-100 dark:bg-zinc-700 rounded w-full"></div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
