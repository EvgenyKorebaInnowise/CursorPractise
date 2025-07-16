import { useState } from 'react'
import { useAppStore } from '../store/useAppStore'

function extractVideoId(urlOrId: string): string | null {
  // Handles full YouTube URLs and raw IDs
  const regex =
    /(?:youtube\.com.*[?&]v=|youtu\.be\/|youtube\.com\/embed\/)([a-zA-Z0-9_-]{11})/
  if (urlOrId.length === 11 && /^[a-zA-Z0-9_-]+$/.test(urlOrId)) return urlOrId
  const match = urlOrId.match(regex)
  return match ? match[1] : null
}

export const VideoInputForm = () => {
  const [input, setInput] = useState('')
  const setVideoId = useAppStore(s => s.setVideoId)

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    const id = extractVideoId(input.trim())
    if (id) {
      setVideoId(id)
    } else {
      alert('Please enter a valid YouTube video URL or ID.')
    }
  }

  return (
    <form
      onSubmit={handleSubmit}
      className='flex items-center gap-2 mb-6'
      autoComplete='off'
    >
      <input
        type='text'
        className='input input-bordered w-full max-w-md px-3 py-2 rounded border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-800 dark:text-gray-100 dark:border-gray-700'
        placeholder='Paste YouTube URL or ID'
        value={input}
        onChange={e => setInput(e.target.value)}
      />
      <button
        type='submit'
        className='btn bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 transition'
      >
        Load
      </button>
    </form>
  )
}
