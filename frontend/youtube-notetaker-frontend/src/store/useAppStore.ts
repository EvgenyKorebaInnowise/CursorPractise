import { create } from 'zustand'

interface AppState {
  videoId: string | null
  player: any | null
  setVideoId: (videoId: string | null) => void
  setPlayer: (player: any | null) => void
}

export const useAppStore = create<AppState>(set => ({
  videoId: null,
  player: null,
  setVideoId: videoId => set({ videoId }),
  setPlayer: player => set({ player }),
}))
