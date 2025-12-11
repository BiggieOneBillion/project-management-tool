import { MessageCircle } from 'lucide-react';
// import PropTypes from 'prop-types';
import CommentForm from './CommentForm';
import CommentReply from './CommentReply';
import { useCreateComment } from '../hooks';

const CommentThread = ({ taskId, comments = [], currentUserId, isLoading }) => {
  const { mutate: createComment, isPending: isCreating } = useCreateComment();

  const handleCreateComment = (content) => {
    createComment({
      taskId,
      data: {
        content,
        parentId: null, // Root comment
      },
    });
  };

  const handleReply = (parentId, content) => {
    createComment({
      taskId,
      data: {
        content,
        parentId,
      },
    });
  };

  console.log("COMMENTS---", comments)

  return (
    <div className="p-5 rounded-md border border-gray-300 dark:border-zinc-800 flex flex-col lg:h-[80vh]">
      <h2 className="text-base font-semibold flex items-center gap-2 mb-4 text-gray-900 dark:text-white">
        <MessageCircle className="size-5" />
        Task Discussion ({comments.length})
      </h2>

      {/* Comments list */}
      <div className="flex-1 md:overflow-y-scroll no-scrollbar">
        {isLoading ? (
          <p className="text-gray-600 dark:text-zinc-500 mb-4 text-sm">
            Loading comments...
          </p>
        ) : comments.length > 0 ? (
          <div className="flex flex-col gap-4 mb-6 mr-2">
            {comments.map((comment) => (
              <CommentReply
                key={comment.id}
                comment={comment}
                currentUserId={currentUserId}
                onReply={handleReply}
                level={0}
              />
            ))}
          </div>
        ) : (
          <p className="text-gray-600 dark:text-zinc-500 mb-4 text-sm">
            No comments yet. Be the first!
          </p>
        )}
      </div>

      {/* Add new comment */}
      <div className="mt-4 pt-4 border-t border-gray-200 dark:border-zinc-700">
        <CommentForm
          onSubmit={handleCreateComment}
          placeholder="Write a comment..."
          submitLabel="Post"
          isSubmitting={isCreating}
        />
      </div>
    </div>
  );
};

// CommentThread.propTypes = {
//   taskId: PropTypes.string.isRequired,
//   comments: PropTypes.array,
//   currentUserId: PropTypes.string,
//   isLoading: PropTypes.bool,
// };

export default CommentThread;
