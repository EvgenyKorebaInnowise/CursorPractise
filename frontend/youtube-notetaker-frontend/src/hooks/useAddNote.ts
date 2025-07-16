import { useMutation, useQueryClient } from '@tanstack/react-query'
import { addNote, type CreateNoteRequest } from '../api/notesApi'

export const useAddNote = (videoId: string | null) => {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (note: CreateNoteRequest) => addNote({ videoId: videoId!, note }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notes', videoId] })
    },
  })
}
