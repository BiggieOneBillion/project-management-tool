import { useState } from 'react';
import { Bell, X } from 'lucide-react';
import { usePendingInvitations } from '../hooks';
import { useAuthStore } from '../stores/useAuthStore';
import InvitationsModal from './InvitationsModal';

export default function InvitationBanner() {
  const { user } = useAuthStore();
  const [isModalOpen, setIsModalOpen] = useState(false);
  
  const { data: invitations = [], isLoading } = usePendingInvitations(user?.email);

  // Don't show banner if no invitations or still loading
  if (isLoading || invitations.length === 0) {
    return null;
  }

  return (
    <>
      <div className="mb-6 bg-blue-50 dark:bg-blue-900/20 border border-blue-200 dark:border-blue-800 rounded-lg p-4">
        <div className="flex items-center justify-between gap-4">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-blue-100 dark:bg-blue-800/50 rounded-full">
              <Bell className="w-5 h-5 text-blue-600 dark:text-blue-400" />
            </div>
            <div>
              <p className="text-sm font-medium text-blue-900 dark:text-blue-100">
                You have {invitations.length} pending workspace invitation{invitations.length !== 1 ? 's' : ''}
              </p>
              <p className="text-xs text-blue-700 dark:text-blue-300">
                Review and respond to join new workspaces
              </p>
            </div>
          </div>
          <button
            onClick={() => setIsModalOpen(true)}
            className="px-4 py-2 text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-lg transition-colors"
          >
            View Invitations
          </button>
        </div>
      </div>

      <InvitationsModal 
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        invitations={invitations}
      />
    </>
  );
}
