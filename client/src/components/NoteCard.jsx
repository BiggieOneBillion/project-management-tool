import { useState } from 'react';
import { format } from 'date-fns';
import { MoreVertical, Edit2, Trash2, FileImage } from 'lucide-react';
import NoteEditor from './NoteEditor';
import { useUpdateNote, useDeleteNote, useDeleteAttachment } from '../hooks';

const NoteCard = ({ note, taskId, currentUserId }) => {
  const [isEditing, setIsEditing] = useState(false);
  const [showMenu, setShowMenu] = useState(false);
  
  const { mutate: updateNote, isPending: isUpdating } = useUpdateNote();
  const { mutate: deleteNote, isPending: isDeleting } = useDeleteNote();
  const { mutate: deleteAttachment } = useDeleteAttachment();
  
  const isOwner = note.userId === currentUserId;
  
  const handleUpdate = (content) => {
    updateNote(
      {
        taskId,
        noteId: note.id,
        data: {
          title: note.title,
          content,
          mentionedUserIds: note.mentions?.map(m => m.mentionedUserId) || []
        }
      },
      {
        onSuccess: () => setIsEditing(false)
      }
    );
  };
  
  const handleDelete = () => {
    if (window.confirm('Are you sure you want to delete this note? This will also delete all attachments.')) {
      deleteNote({ taskId, noteId: note.id });
    }
    setShowMenu(false);
  };
  
  const handleDeleteAttachment = (attachmentId) => {
    if (window.confirm('Are you sure you want to delete this attachment?')) {
      deleteAttachment({ taskId, noteId: note.id, attachmentId });
    }
  };
  
  return (
    <div className="bg-white dark:bg-gradient-to-br dark:from-zinc-800 dark:to-zinc-900 border border-gray-300 dark:border-zinc-700 rounded-lg p-4">
      {/* Header */}
      <div className="flex items-start justify-between mb-3">
        <div className="flex items-center gap-2">
          <img
            src={note.user?.image || '/default-avatar.png'}
            alt={note.user?.name || 'User'}
            className="size-8 rounded-full"
          />
          <div>
            <p className="font-medium text-gray-900 dark:text-white text-sm">
              {note.user?.name || 'Unknown User'}
            </p>
            <p className="text-xs text-gray-500 dark:text-zinc-400">
              {format(new Date(note.createdAt), 'MMM dd, yyyy • HH:mm')}
              {note.createdAt !== note.updatedAt && (
                <span className="italic ml-1">(edited)</span>
              )}
            </p>
          </div>
        </div>
        
        {isOwner && (
          <div className="relative">
            <button
              onClick={() => setShowMenu(!showMenu)}
              className="p-1 hover:bg-gray-100 dark:hover:bg-zinc-700 rounded"
            >
              <MoreVertical className="size-4 text-gray-500 dark:text-zinc-400" />
            </button>
            
            {showMenu && (
              <div className="absolute right-0 mt-1 w-32 bg-white dark:bg-zinc-800 border border-gray-200 dark:border-zinc-700 rounded-md shadow-lg z-10">
                <button
                  onClick={() => {
                    setIsEditing(true);
                    setShowMenu(false);
                  }}
                  className="w-full flex items-center gap-2 px-3 py-2 text-sm text-gray-700 dark:text-zinc-300 hover:bg-gray-100 dark:hover:bg-zinc-700"
                >
                  <Edit2 className="size-3" />
                  Edit
                </button>
                <button
                  onClick={handleDelete}
                  disabled={isDeleting}
                  className="w-full flex items-center gap-2 px-3 py-2 text-sm text-red-600 dark:text-red-400 hover:bg-gray-100 dark:hover:bg-zinc-700"
                >
                  <Trash2 className="size-3" />
                  Delete
                </button>
              </div>
            )}
          </div>
        )}
      </div>
      
      {/* Title */}
      {note.title && (
        <h3 className="text-lg font-semibold text-gray-900 dark:text-white mb-2">
          {note.title}
        </h3>
      )}
      
      {/* Content */}
      {isEditing ? (
        <NoteEditor
          initialContent={note.content}
          onSubmit={handleUpdate}
          onCancel={() => setIsEditing(false)}
          isSubmitting={isUpdating}
          submitLabel="Update"
        />
      ) : (
        <div 
          className="prose prose-sm dark:prose-invert max-w-none"
          dangerouslySetInnerHTML={{ __html: note.content }}
        />
      )}
      
      {/* Attachments */}
      {note.attachments && note.attachments.length > 0 && !isEditing && (
        <div className="mt-4 space-y-2">
          <p className="text-xs font-medium text-gray-600 dark:text-zinc-400 flex items-center gap-1">
            <FileImage className="size-3" />
            Attachments ({note.attachments.length})
          </p>
          <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
            {note.attachments.map((attachment) => (
              <div key={attachment.id} className="relative group">
                <img
                  src={attachment.filePath}
                  alt={attachment.fileName}
                  className="w-full h-32 object-cover rounded border border-gray-300 dark:border-zinc-700"
                />
                {isOwner && (
                  <button
                    onClick={() => handleDeleteAttachment(attachment.id)}
                    className="absolute top-1 right-1 p-1 bg-red-600 text-white rounded opacity-0 group-hover:opacity-100 transition-opacity"
                  >
                    <Trash2 className="size-3" />
                  </button>
                )}
              </div>
            ))}
          </div>
        </div>
      )}
      
      {/* Mentions */}
      {note.mentions && note.mentions.length > 0 && !isEditing && (
        <div className="mt-3 pt-3 border-t border-gray-200 dark:border-zinc-700">
          <p className="text-xs text-gray-500 dark:text-zinc-400">
            Mentioned: {note.mentions.map(m => m.mentionedUser?.name).filter(Boolean).join(', ')}
          </p>
        </div>
      )}
    </div>
  );
};

export default NoteCard;
