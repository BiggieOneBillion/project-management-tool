// import PropTypes from 'prop-types';
import CommentItem from './CommentItem';

const CommentReply = ({ comment, currentUserId, onReply, level }) => {
  return (
    <CommentItem
      comment={comment}
      currentUserId={currentUserId}
      onReply={onReply}
      level={level}
    >
      {/* Recursively render replies */}
      {comment.replies && comment.replies.length > 0 && (
        <div className="space-y-2">
          {comment.replies.map((reply) => (
            <CommentReply
              key={reply.id}
              comment={reply}
              currentUserId={currentUserId}
              onReply={onReply}
              level={level + 1}
            />
          ))}
        </div>
      )}
    </CommentItem>
  );
};

// CommentReply.propTypes = {
//   comment: PropTypes.object.isRequired,
//   currentUserId: PropTypes.string,
//   onReply: PropTypes.func.isRequired,
//   level: PropTypes.number.isRequired,
// };

export default CommentReply;
