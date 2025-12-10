import { useState } from 'react';
import { StickyNote, Plus } from 'lucide-react';
import NoteCard from './NoteCard';
import NoteEditor from './NoteEditor';
import { useCreateNote } from '../hooks';

const NotesList = ({ taskId, notes = [], currentUserId, isLoading }) => {
  const [isCreating, setIsCreating] = useState(false);
  const { mutate: createNote, isPending: isSubmitting } = useCreateNote();
  
  const handleCreate = (content) => {
    createNote(
      {
        taskId,
        data: {
          title: '',
          content,
          mentionedUserIds: []
        }
      },
      {
        onSuccess: () => setIsCreating(false)
      }
    );
  };
  
  return (
    <div className="space-y-4">
      {/* Header */}
      <div className="flex items-center justify-between">
        <h2 className="text-lg font-semibold flex items-center gap-2 text-gray-900 dark:text-white">
          <StickyNote className="size-5" />
          Notes ({notes.length})
        </h2>
        <button
          onClick={() => setIsCreating(!isCreating)}
          className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white text-sm rounded hover:bg-blue-700"
        >
          <Plus className="size-4" />
          New Note
        </button>
      </div>
      
      {/* Create Note Form */}
      {isCreating && (
        <NoteEditor
          onSubmit={handleCreate}
          onCancel={() => setIsCreating(false)}
          isSubmitting={isSubmitting}
          placeholder="Write your note here... Use the toolbar to format text, add images, and more."
          submitLabel="Create Note"
        />
      )}
      
      {/* Notes List */}
      {isLoading ? (
        <div className="text-center py-8">
          <p className="text-gray-600 dark:text-zinc-400">Loading notes...</p>
        </div>
      ) : notes.length > 0 ? (
        <div className="space-y-4">
          {notes.map((note) => (
            <NoteCard
              key={note.id}
              note={note}
              taskId={taskId}
              currentUserId={currentUserId}
            />
          ))}
        </div>
      ) : (
        <div className="text-center py-12 border-2 border-dashed border-gray-300 dark:border-zinc-700 rounded-lg">
          <StickyNote className="size-12 mx-auto text-gray-400 dark:text-zinc-600 mb-3" />
          <p className="text-gray-600 dark:text-zinc-400 mb-2">No notes yet</p>
          <p className="text-sm text-gray-500 dark:text-zinc-500">
            Create your first note to get started
          </p>
        </div>
      )}
    </div>
  );
};

export default NotesList;
