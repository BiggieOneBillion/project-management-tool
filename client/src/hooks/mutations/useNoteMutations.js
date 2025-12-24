import { useMutation, useQueryClient } from '@tanstack/react-query';
import { noteService } from '../../services';
import toast from 'react-hot-toast';

export function useCreateNote() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, data }) => noteService.create(taskId, data),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries(['notes', 'task', taskId]);
      toast.success('Note created');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to create note');
    },
  });
}

export function useUpdateNote() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, noteId, data }) => noteService.update(taskId, noteId, data),
    onSuccess: (_, { taskId, noteId }) => {
      queryClient.invalidateQueries(['notes', 'task', taskId]);
      queryClient.invalidateQueries(['notes', noteId]);
      toast.success('Note updated');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to update note');
    },
  });
}

export function useDeleteNote() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, noteId }) => noteService.delete(taskId, noteId),
    onSuccess: (_, { taskId }) => {
      queryClient.invalidateQueries(['notes', 'task', taskId]);
      toast.success('Note deleted');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to delete note');
    },
  });
}

export function useUploadAttachment() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, noteId, file }) => noteService.uploadAttachment(taskId, noteId, file),
    onSuccess: (_, { taskId, noteId }) => {
      queryClient.invalidateQueries(['notes', 'task', taskId]);
      queryClient.invalidateQueries(['notes', noteId]);
      toast.success('File uploaded');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to upload file');
    },
  });
}

export function useDeleteAttachment() {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: ({ taskId, noteId, attachmentId }) => 
      noteService.deleteAttachment(taskId, noteId, attachmentId),
    onSuccess: (_, { taskId, noteId }) => {
      queryClient.invalidateQueries(['notes', 'task', taskId]);
      queryClient.invalidateQueries(['notes', noteId]);
      toast.success('Attachment deleted');
    },
    onError: (error) => {
      toast.error(error?.message || 'Failed to delete attachment');
    },
  });
}
