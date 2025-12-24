import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { noteService } from '../../services';
import toast from 'react-hot-toast';

export function useTaskNotes(taskId) {
  return useQuery({
    queryKey: ['notes', 'task', taskId],
    queryFn: () => noteService.getTaskNotes(taskId),
    enabled: !!taskId,
  });
}

export function useNote(taskId, noteId) {
  return useQuery({
    queryKey: ['notes', noteId],
    queryFn: () => noteService.getNoteById(taskId, noteId),
    enabled: !!taskId && !!noteId,
  });
}
