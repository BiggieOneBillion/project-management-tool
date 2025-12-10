import { useState } from 'react';
import { Bell } from 'lucide-react';
import { useNotifications, useUnreadCount, useMarkAsRead, useMarkAllAsRead } from '../hooks';
import { format } from 'date-fns';

const NotificationBadge = () => {
  const [isOpen, setIsOpen] = useState(false);
  const { data: unreadCount } = useUnreadCount();
  const { data: notifications } = useNotifications(true); // Only unread
  const { mutate: markAsRead } = useMarkAsRead();
  const { mutate: markAllAsRead } = useMarkAllAsRead();
  
  const count = unreadCount?.data || 0;
  const notificationList = notifications?.data || [];
  
  const handleNotificationClick = (notification) => {
    markAsRead(notification.id);
    // TODO: Navigate to related entity if needed
    setIsOpen(false);
  };
  
  const handleMarkAllRead = () => {
    markAllAsRead();
  };
  
  return (
    <div className="relative">
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="relative p-2 hover:bg-gray-100 dark:hover:bg-zinc-800 rounded-full"
      >
        <Bell className="size-5 text-gray-700 dark:text-zinc-300" />
        {count > 0 && (
          <span className="absolute top-0 right-0 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white transform translate-x-1/2 -translate-y-1/2 bg-red-600 rounded-full">
            {count > 99 ? '99+' : count}
          </span>
        )}
      </button>
      
      {isOpen && (
        <>
          {/* Backdrop */}
          <div
            className="fixed inset-0 z-10"
            onClick={() => setIsOpen(false)}
          />
          
          {/* Dropdown */}
          <div className="absolute right-0 mt-2 w-80 bg-white dark:bg-zinc-900 border border-gray-200 dark:border-zinc-700 rounded-lg shadow-lg z-20 max-h-96 overflow-hidden flex flex-col">
            {/* Header */}
            <div className="flex items-center justify-between p-4 border-b border-gray-200 dark:border-zinc-700">
              <h3 className="font-semibold text-gray-900 dark:text-white">
                Notifications
              </h3>
              {count > 0 && (
                <button
                  onClick={handleMarkAllRead}
                  className="text-xs text-blue-600 dark:text-blue-400 hover:underline"
                >
                  Mark all as read
                </button>
              )}
            </div>
            
            {/* Notifications List */}
            <div className="overflow-y-auto flex-1">
              {notificationList.length > 0 ? (
                <div className="divide-y divide-gray-200 dark:divide-zinc-700">
                  {notificationList.map((notification) => (
                    <button
                      key={notification.id}
                      onClick={() => handleNotificationClick(notification)}
                      className="w-full text-left p-4 hover:bg-gray-50 dark:hover:bg-zinc-800 transition-colors"
                    >
                      <p className="font-medium text-sm text-gray-900 dark:text-white mb-1">
                        {notification.title}
                      </p>
                      <p className="text-xs text-gray-600 dark:text-zinc-400 mb-2">
                        {notification.message}
                      </p>
                      <p className="text-xs text-gray-500 dark:text-zinc-500">
                        {format(new Date(notification.createdAt), 'MMM dd, HH:mm')}
                      </p>
                    </button>
                  ))}
                </div>
              ) : (
                <div className="p-8 text-center">
                  <Bell className="size-12 mx-auto text-gray-400 dark:text-zinc-600 mb-3" />
                  <p className="text-gray-600 dark:text-zinc-400 text-sm">
                    No new notifications
                  </p>
                </div>
              )}
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default NotificationBadge;
