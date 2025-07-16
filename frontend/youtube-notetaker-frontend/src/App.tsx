import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { NotesSection } from './components/NotesSection'
import { VideoInputForm } from './components/VideoInputForm'
import { YouTubePlayerWrapper } from './components/YouTubePlayerWrapper'
import './index.css'
import { useAppStore } from './store/useAppStore'

const queryClient = new QueryClient()

function App() {
  const videoId = useAppStore(s => s.videoId)

  return (
    <QueryClientProvider client={queryClient}>
      <div className='min-h-screen bg-gray-100 dark:bg-gray-900 py-8 px-4 flex flex-col items-center'>
        <h1 className='text-3xl font-bold mb-6 text-center text-blue-700 dark:text-blue-400'>
          YouTube Video Notetaker
        </h1>
        <VideoInputForm />
        {videoId && (
          <>
            <YouTubePlayerWrapper />
            <NotesSection />
          </>
        )}
      </div>
    </QueryClientProvider>
  )
}

export default App
