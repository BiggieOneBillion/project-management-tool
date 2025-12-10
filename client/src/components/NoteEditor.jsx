import { useEditor, EditorContent } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import Image from '@tiptap/extension-image';
import Link from '@tiptap/extension-link';
import Placeholder from '@tiptap/extension-placeholder';
import { 
  Bold, Italic, List, ListOrdered, Code, Quote, Heading2, 
  Link as LinkIcon, Image as ImageIcon, Undo, Redo 
} from 'lucide-react';

const NoteEditor = ({ 
  initialContent = '', 
  onSubmit, 
  onCancel,
  isSubmitting = false,
  placeholder = 'Write your note...',
  submitLabel = 'Save Note'
}) => {
  const editor = useEditor({
    extensions: [
      StarterKit,
      Image,
      Link.configure({
        openOnClick: false,
      }),
      Placeholder.configure({
        placeholder,
      }),
    ],
    content: initialContent,
    editorProps: {
      attributes: {
        class: 'prose prose-sm dark:prose-invert max-w-none focus:outline-none min-h-[200px] p-4',
      },
    },
  });

  const handleSubmit = () => {
    if (!editor) return;
    const content = editor.getHTML();
    if (content.trim() === '<p></p>' || content.trim() === '') {
      return;
    }
    onSubmit(content);
  };

  const addImage = () => {
    const url = window.prompt('Enter image URL:');
    if (url && editor) {
      editor.chain().focus().setImage({ src: url }).run();
    }
  };

  const setLink = () => {
    const url = window.prompt('Enter URL:');
    if (url && editor) {
      editor.chain().focus().setLink({ href: url }).run();
    }
  };

  if (!editor) {
    return null;
  }

  return (
    <div className="border border-gray-300 dark:border-zinc-700 rounded-md overflow-hidden">
      {/* Toolbar */}
      <div className="flex flex-wrap items-center gap-1 p-2 border-b border-gray-300 dark:border-zinc-700 bg-gray-50 dark:bg-zinc-800">
        <button
          onClick={() => editor.chain().focus().toggleBold().run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('bold') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Bold"
        >
          <Bold className="size-4" />
        </button>
        
        <button
          onClick={() => editor.chain().focus().toggleItalic().run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('italic') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Italic"
        >
          <Italic className="size-4" />
        </button>
        
        <div className="w-px h-6 bg-gray-300 dark:bg-zinc-600 mx-1" />
        
        <button
          onClick={() => editor.chain().focus().toggleHeading({ level: 2 }).run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('heading', { level: 2 }) ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Heading"
        >
          <Heading2 className="size-4" />
        </button>
        
        <button
          onClick={() => editor.chain().focus().toggleBulletList().run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('bulletList') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Bullet List"
        >
          <List className="size-4" />
        </button>
        
        <button
          onClick={() => editor.chain().focus().toggleOrderedList().run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('orderedList') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Numbered List"
        >
          <ListOrdered className="size-4" />
        </button>
        
        <div className="w-px h-6 bg-gray-300 dark:bg-zinc-600 mx-1" />
        
        <button
          onClick={() => editor.chain().focus().toggleCodeBlock().run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('codeBlock') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Code Block"
        >
          <Code className="size-4" />
        </button>
        
        <button
          onClick={() => editor.chain().focus().toggleBlockquote().run()}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('blockquote') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Quote"
        >
          <Quote className="size-4" />
        </button>
        
        <div className="w-px h-6 bg-gray-300 dark:bg-zinc-600 mx-1" />
        
        <button
          onClick={setLink}
          className={`p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 ${
            editor.isActive('link') ? 'bg-gray-200 dark:bg-zinc-700' : ''
          }`}
          title="Add Link"
        >
          <LinkIcon className="size-4" />
        </button>
        
        <button
          onClick={addImage}
          className="p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700"
          title="Add Image"
        >
          <ImageIcon className="size-4" />
        </button>
        
        <div className="w-px h-6 bg-gray-300 dark:bg-zinc-600 mx-1" />
        
        <button
          onClick={() => editor.chain().focus().undo().run()}
          disabled={!editor.can().undo()}
          className="p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 disabled:opacity-50 disabled:cursor-not-allowed"
          title="Undo"
        >
          <Undo className="size-4" />
        </button>
        
        <button
          onClick={() => editor.chain().focus().redo().run()}
          disabled={!editor.can().redo()}
          className="p-2 rounded hover:bg-gray-200 dark:hover:bg-zinc-700 disabled:opacity-50 disabled:cursor-not-allowed"
          title="Redo"
        >
          <Redo className="size-4" />
        </button>
      </div>

      {/* Editor */}
      <EditorContent editor={editor} />

      {/* Actions */}
      <div className="flex justify-end gap-2 p-3 border-t border-gray-300 dark:border-zinc-700 bg-gray-50 dark:bg-zinc-800">
        {onCancel && (
          <button
            onClick={onCancel}
            className="px-4 py-2 text-sm text-gray-700 dark:text-zinc-300 hover:bg-gray-200 dark:hover:bg-zinc-700 rounded"
            disabled={isSubmitting}
          >
            Cancel
          </button>
        )}
        <button
          onClick={handleSubmit}
          disabled={isSubmitting}
          className="px-4 py-2 text-sm bg-blue-600 text-white rounded hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {isSubmitting ? 'Saving...' : submitLabel}
        </button>
      </div>
    </div>
  );
};

export default NoteEditor;
