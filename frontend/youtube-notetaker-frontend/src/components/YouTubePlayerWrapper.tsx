import YouTube, { type YouTubeProps } from 'react-youtube'
import { useAppStore } from '../store/useAppStore'

export const YouTubePlayerWrapper = () => {
  const videoId = useAppStore(s => s.videoId)
  const setPlayer = useAppStore(s => s.setPlayer)

  if (!videoId) return null

  const onReady: YouTubeProps['onReady'] = event => {
    setPlayer(event.target)
  }

  return (
    <div className='w-full max-w-2xl mx-auto mb-6'>
      <YouTube
        videoId={videoId}
        opts={{
          width: '100%',
          height: '390',
          playerVars: { modestbranding: 1, rel: 0 },
        }}
        onReady={onReady}
      />
    </div>
  )
}
