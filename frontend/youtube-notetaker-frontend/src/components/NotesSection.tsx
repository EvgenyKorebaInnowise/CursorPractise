import { useRef, useState } from 'react'
import { useAddNote } from '../hooks/useAddNote'
import { useNotes } from '../hooks/useNotes'
import { useAppStore } from '../store/useAppStore'

export const NotesSection = () => {
  const videoId = useAppStore(s => s.videoId)
  const player = useAppStore(s => s.player)
  const { data: notes, isLoading, isError } = useNotes(videoId)
  const addNoteMutation = useAddNote(videoId)

  const [text, setText] = useState('')
  const [timestamp, setTimestamp] = useState<number | null>(null)
  const inputRef = useRef<HTMLInputElement>(null)

  const handleAddNote = (e: React.FormEvent) => {
    e.preventDefault()
    if (!videoId || !text || timestamp === null) return
    addNoteMutation.mutate(
      { timestampSeconds: timestamp, text },
      {
        onSuccess: () => {
          setText('')
          setTimestamp(null)
          inputRef.current?.focus()
        },
      }
    )
  }

  const handleGetCurrentTime = () => {
    if (player && typeof player.getCurrentTime === 'function') {
      setTimestamp(Math.floor(player.getCurrentTime()))
    }
  }

  const handleNoteClick = (ts: number) => {
    if (player && typeof player.seekTo === 'function') {
      player.seekTo(ts, true)
    }
  }

  return (
    <section className='w-full max-w-2xl mx-auto'>
      <form
        onSubmit={handleAddNote}
        className='flex flex-col md:flex-row gap-2 mb-4'
      >
        <input
          ref={inputRef}
          type='text'
          className='input input-bordered flex-1 px-3 py-2 rounded border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-gray-100 dark:border-gray-700'
          placeholder='Write a note...'
          value={text}
          onChange={e => setText(e.target.value)}
          required
        />
        <div className='flex gap-2'>
          <input
            type='number'
            className='input input-bordered w-24 px-2 py-2 rounded border border-gray-300 focus:outline-none dark:bg-gray-800 dark:text-gray-100 dark:border-gray-700'
            placeholder='Time (s)'
            value={timestamp ?? ''}
            onChange={e => setTimestamp(Number(e.target.value))}
            min={0}
            required
          />
          <button
            type='button'
            className='btn bg-gray-200 px-3 py-2 rounded hover:bg-gray-300 dark:bg-gray-700 dark:text-gray-100 dark:hover:bg-gray-600'
            onClick={handleGetCurrentTime}
          >
            Get Time
          </button>
        </div>
        <button
          type='submit'
          className='btn bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition'
          disabled={addNoteMutation.isPending}
        >
          Add Note
        </button>
      </form>
      <div className='bg-white rounded shadow p-4 dark:bg-gray-800 dark:text-gray-100'>
        <h2 className='text-lg font-semibold mb-2'>Notes</h2>
        {isLoading && <div>Loading notes...</div>}
        {isError && <div className='text-red-500'>Failed to load notes.</div>}
        <ul className='space-y-2'>
          {notes &&
            notes.map(note => (
              <li
                key={note.id}
                className='flex items-center gap-3 cursor-pointer hover:bg-blue-50 rounded px-2 py-1 transition dark:hover:bg-gray-700'
                onClick={() => handleNoteClick(note.timestampSeconds)}
                title='Click to jump to this time'
              >
                <span className='text-blue-600 font-mono w-16 dark:text-blue-400'>
                  {note.timestampSeconds}s
                </span>
                <span className='flex-1'>{note.text}</span>
                <span className='text-xs text-gray-400 dark:text-gray-500'>
                  {new Date(note.createdAt).toLocaleString()}
                </span>
              </li>
            ))}
          {notes && notes.length === 0 && (
            <li className='text-gray-500 dark:text-gray-400'>No notes yet.</li>
          )}
        </ul>
      </div>
    </section>
  )
}
