import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL as string

export interface Note {
  id: string
  timestampSeconds: number
  text: string
  createdAt: string
}

export interface CreateNoteRequest {
  timestampSeconds: number
  text: string
}

export const fetchNotes = async (videoId: string): Promise<Note[]> => {
  const { data } = await axios.get<Note[]>(`${API_BASE_URL}/${videoId}`)
  return data
}

export const addNote = async ({
  videoId,
  note,
}: {
  videoId: string
  note: CreateNoteRequest
}): Promise<Note[]> => {
  const { data } = await axios.post<Note[]>(`${API_BASE_URL}/${videoId}`, note)
  return data
}
