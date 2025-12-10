import { useState } from 'react';
// import PropTypes from 'prop-types';

const CommentForm = ({ 
  initialValue = '', 
  onSubmit, 
  onCancel, 
  placeholder = 'Write a comment...', 
  submitLabel = 'Post',
  isSubmitting = false 
}) => {
  const [content, setContent] = useState(initialValue);
  const maxLength = 5000;

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!content.trim()) return;
    onSubmit(content);
  };

  const handleCancel = () => {
    setContent(initialValue);
    if (onCancel) onCancel();
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-2">
      <div className="relative">
        <textarea
          value={content}
          onChange={(e) => setContent(e.target.value)}
          placeholder={placeholder}
          className="w-full dark:bg-zinc-800 border border-gray-300 dark:border-zinc-700 rounded-md p-2 text-sm text-gray-900 dark:text-zinc-200 resize-none focus:outline-none focus:ring-1 focus:ring-blue-600"
          rows={3}
          maxLength={maxLength}
          disabled={isSubmitting}
        />
        <div className="absolute bottom-2 right-2 text-xs text-gray-400 dark:text-zinc-600">
          {content.length}/{maxLength}
        </div>
      </div>
      
      <div className="flex gap-2 justify-end">
        {onCancel && (
          <button
            type="button"
            onClick={handleCancel}
            disabled={isSubmitting}
            className="px-4 py-1.5 text-sm text-gray-700 dark:text-zinc-300 hover:bg-gray-100 dark:hover:bg-zinc-800 rounded border border-gray-300 dark:border-zinc-700 disabled:opacity-50"
          >
            Cancel
          </button>
        )}
        <button
          type="submit"
          disabled={!content.trim() || isSubmitting}
          className="bg-gradient-to-l from-blue-500 to-blue-600 transition-colors text-white text-sm px-5 py-1.5 rounded disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {isSubmitting ? 'Posting...' : submitLabel}
        </button>
      </div>
    </form>
  );
};

// CommentForm.propTypes = {
//   initialValue: PropTypes.string,
//   onSubmit: PropTypes.func.isRequired,
//   onCancel: PropTypes.func,
//   placeholder: PropTypes.string,
//   submitLabel: PropTypes.string,
//   isSubmitting: PropTypes.bool,
// };

export default CommentForm;
