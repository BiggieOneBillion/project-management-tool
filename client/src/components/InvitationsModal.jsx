import { X, Mail, Calendar, CheckCircle, XCircle } from 'lucide-react';
import { useAcceptInvitation, useRejectInvitation } from '../hooks';

export default function InvitationsModal({ isOpen, onClose, invitations }) {
  const { mutate: acceptInvitation, isPending: isAccepting } = useAcceptInvitation();
  const { mutate: rejectInvitation, isPending: isRejecting } = useRejectInvitation();

  if (!isOpen) return null;

  const handleAccept = (invitation) => {
    acceptInvitation(invitation.token, {
      onSuccess: () => {
        // Close modal if no more invitations
        if (invitations.length === 1) {
          onClose();
        }
      }
    });
  };

  const handleReject = (invitation) => {
    rejectInvitation(invitation.id, {
      onSuccess: () => {
        // Close modal if no more invitations
        if (invitations.length === 1) {
          onClose();
        }
      }
    });
  };

  return (
    <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-zinc-900 rounded-xl shadow-2xl w-full max-w-2xl max-h-[80vh] overflow-hidden border border-gray-200 dark:border-zinc-800">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-gray-200 dark:border-zinc-800">
          <div>
            <h2 className="text-xl font-semibold text-gray-900 dark:text-white">
              Workspace Invitations
            </h2>
            <p className="text-sm text-gray-500 dark:text-zinc-400 mt-1">
              {invitations.length} pending invitation{invitations.length !== 1 ? 's' : ''}
            </p>
          </div>
          <button
            onClick={onClose}
            className="p-2 hover:bg-gray-100 dark:hover:bg-zinc-800 rounded-lg transition-colors"
          >
            <X className="w-5 h-5 text-gray-500 dark:text-zinc-400" />
          </button>
        </div>

        {/* Invitations List */}
        <div className="overflow-y-auto max-h-[calc(80vh-140px)] p-6 space-y-4">
          {invitations.map((invitation) => (
            <div
              key={invitation.id}
              className="p-4 border border-gray-200 dark:border-zinc-800 rounded-lg hover:border-blue-300 dark:hover:border-blue-700 transition-colors"
            >
              <div className="flex items-start justify-between gap-4">
                <div className="flex-1">
                  <h3 className="font-medium text-gray-900 dark:text-white mb-2">
                    {invitation.workspaceName || 'Workspace Invitation'}
                  </h3>
                  
                  <div className="space-y-1.5 text-sm text-gray-600 dark:text-zinc-400">
                    <div className="flex items-center gap-2">
                      <Mail className="w-4 h-4" />
                      <span>Invited to: {invitation.email}</span>
                    </div>
                    
                    {invitation.invitedBy && (
                      <div className="flex items-center gap-2">
                        <span>By: {invitation.invitedBy}</span>
                      </div>
                    )}
                    
                    {invitation.createdAt && (
                      <div className="flex items-center gap-2">
                        <Calendar className="w-4 h-4" />
                        <span>{new Date(invitation.createdAt).toLocaleDateString()}</span>
                      </div>
                    )}
                  </div>
                </div>

                {/* Action Buttons */}
                <div className="flex gap-2">
                  <button
                    onClick={() => handleAccept(invitation)}
                    disabled={isAccepting || isRejecting}
                    className="flex items-center gap-1.5 px-3 py-2 text-sm font-medium text-white bg-green-600 hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed rounded-lg transition-colors"
                  >
                    <CheckCircle className="w-4 h-4" />
                    Accept
                  </button>
                  <button
                    onClick={() => handleReject(invitation)}
                    disabled={isAccepting || isRejecting}
                    className="flex items-center gap-1.5 px-3 py-2 text-sm font-medium text-white bg-red-600 hover:bg-red-700 disabled:opacity-50 disabled:cursor-not-allowed rounded-lg transition-colors"
                  >
                    <XCircle className="w-4 h-4" />
                    Reject
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>

        {/* Footer */}
        <div className="p-6 border-t border-gray-200 dark:border-zinc-800">
          <button
            onClick={onClose}
            className="w-full px-4 py-2 text-sm font-medium text-gray-700 dark:text-zinc-300 bg-gray-100 dark:bg-zinc-800 hover:bg-gray-200 dark:hover:bg-zinc-700 rounded-lg transition-colors"
          >
            Close
          </button>
        </div>
      </div>
    </div>
  );
}
