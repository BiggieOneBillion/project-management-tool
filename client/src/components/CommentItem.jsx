import { useState } from 'react';
import { format } from 'date-fns';
import { MoreVertical, Edit2, Trash2, Reply, ChevronDown, ChevronUp } from 'lucide-react';
// import PropTypes from 'prop-types';
import CommentForm from './CommentForm';
import { useUpdateComment, useDeleteComment } from '../hooks';

const CommentItem = ({ 
  comment, 
  currentUserId, 
  onReply, 
  level = 0,
  children 
}) => {
  const [isEditing, setIsEditing] = useState(false);
  const [showMenu, setShowMenu] = useState(false);
  const [showReplies, setShowReplies] = useState(true);
  const [isReplying, setIsReplying] = useState(false);

  const { mutate: updateComment, isPending: isUpdating } = useUpdateComment();
  const { mutate: deleteComment, isPending: isDeleting } = useDeleteComment();

  const isOwner = comment.userId === currentUserId;
  const canReply = level < 4;
  const hasReplies = comment.replies && comment.replies.length > 0;

  // Calculate indent based on level
  const indentClass = level > 0 ? `ml-${Math.min(level * 4, 16)}` : '';

  const handleEdit = () => {
    setIsEditing(true);
    setShowMenu(false);
  };

  const handleDelete = () => {
    if (window.confirm('Are you sure you want to delete this comment? This will also delete all replies.')) {
      deleteComment({ 
        taskId: comment.taskId, 
        commentId: comment.id 
      });
    }
    setShowMenu(false);
  };

  const handleUpdateSubmit = (content) => {
    updateComment(
      { 
        taskId: comment.taskId, 
        commentId: comment.id, 
        data: { content } 
      },
      {
        onSuccess: () => {
          setIsEditing(false);
        }
      }
    );
  };

  const handleReplySubmit = (content) => {
    onReply(comment.id, content);
    setIsReplying(false);
  };

  return (
    <div className={`${indentClass}`}>
      <div className="flex gap-3 mb-3">
        {/* Vertical line for nested comments */}
        {level > 0 && (
          <div className="w-0.5 bg-gray-200 dark:bg-zinc-700 -ml-4" />
        )}
        
        <div className="flex-1">
          <div className="bg-white dark:bg-gradient-to-br dark:from-zinc-800 dark:to-zinc-900 border border-gray-300 dark:border-zinc-700 p-3 rounded-md">
            {/* Header */}
            <div className="flex items-center justify-between mb-2">
              <div className="flex items-center gap-2 text-sm">
                <img
                  src={comment.user?.image || '/default-avatar.png'}
                  alt={comment.user?.name || 'User'}
                  className="size-6 rounded-full"
                />
                <span className="font-medium text-gray-900 dark:text-white">
                  {comment.user?.name || 'Unknown User'}
                </span>
                <span className="text-xs text-gray-400 dark:text-zinc-600">
                  • {format(new Date(comment.createdAt), 'dd MMM yyyy, HH:mm')}
                </span>
                {comment.createdAt !== comment.updatedAt && (
                  <span className="text-xs text-gray-400 dark:text-zinc-600 italic">
                    (edited)
                  </span>
                )}
              </div>

              {/* Actions menu */}
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
                        onClick={handleEdit}
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

            {/* Content */}
            {isEditing ? (
              <CommentForm
                initialValue={comment.content}
                onSubmit={handleUpdateSubmit}
                onCancel={() => setIsEditing(false)}
                placeholder="Edit your comment..."
                submitLabel="Save"
                isSubmitting={isUpdating}
              />
            ) : (
              <p className="text-sm text-gray-900 dark:text-zinc-200 whitespace-pre-wrap">
                {comment.content}
              </p>
            )}

            {/* Reply button */}
            {!isEditing && canReply && (
              <button
                onClick={() => setIsReplying(!isReplying)}
                className="mt-2 flex items-center gap-1 text-xs text-blue-600 dark:text-blue-400 hover:underline"
              >
                <Reply className="size-3" />
                Reply
              </button>
            )}

            {!isEditing && !canReply && (
              <p className="mt-2 text-xs text-gray-400 dark:text-zinc-600 italic">
                Maximum reply depth reached
              </p>
            )}
          </div>

          {/* Reply form */}
          {isReplying && (
            <div className="mt-2 ml-4">
              <CommentForm
                onSubmit={handleReplySubmit}
                onCancel={() => setIsReplying(false)}
                placeholder="Write a reply..."
                submitLabel="Reply"
              />
            </div>
          )}

          {/* Replies */}
          {hasReplies && (
            <div className="mt-2">
              <button
                onClick={() => setShowReplies(!showReplies)}
                className="flex items-center gap-1 text-xs text-gray-600 dark:text-zinc-400 hover:text-gray-900 dark:hover:text-zinc-200 mb-2"
              >
                {showReplies ? (
                  <>
                    <ChevronUp className="size-3" />
                    Hide {comment.replies.length} {comment.replies.length === 1 ? 'reply' : 'replies'}
                  </>
                ) : (
                  <>
                    <ChevronDown className="size-3" />
                    Show {comment.replies.length} {comment.replies.length === 1 ? 'reply' : 'replies'}
                  </>
                )}
              </button>
              
              {showReplies && children}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

// CommentItem.propTypes = {
//   comment: PropTypes.shape({
//     id: PropTypes.string.isRequired,
//     taskId: PropTypes.string.isRequired,
//     userId: PropTypes.string.isRequired,
//     content: PropTypes.string.isRequired,
//     createdAt: PropTypes.string.isRequired,
//     updatedAt: PropTypes.string.isRequired,
//     level: PropTypes.number,
//     user: PropTypes.shape({
//       id: PropTypes.string,
//       name: PropTypes.string,
//       image: PropTypes.string,
//     }),
//     replies: PropTypes.array,
//   }).isRequired,
//   currentUserId: PropTypes.string,
//   onReply: PropTypes.func.isRequired,
//   level: PropTypes.number,
//   children: PropTypes.node,
// };

export default CommentItem;
