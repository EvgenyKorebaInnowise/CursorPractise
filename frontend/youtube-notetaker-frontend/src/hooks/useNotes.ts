import { useQuery } from '@tanstack/react-query'
import { fetchNotes } from '../api/notesApi'

export const useNotes = (videoId: string | null) => {
  return useQuery({
    queryKey: ['notes', videoId],
    queryFn: () => fetchNotes(videoId!),
    enabled: !!videoId,
  })
}
