// usePlayerStore.ts
import { defineStore } from 'pinia'

export const usePlayerStore = defineStore('player', {
  state: () => ({
    playlist: [] as Song[],
    originalPlaylist: [] as Song[],
    currentIndex: 0,
  }),
  getters: {
    currentSong: (state) => state.playlist[state.currentIndex] || null,
  },
  actions: {
    setPlaylist(songs: Song[]) {
      this.playlist = songs
      this.originalPlaylist = [...songs]
      this.currentIndex = 0
    },
    playSongByIndex(index: number) {
      if (index >= 0 && index < this.playlist.length) {
        this.currentIndex = index
      }
    },
    shufflePlaylist() {
      const current = this.playlist[this.currentIndex]
      const remaining = this.playlist.filter((_, i) => i !== this.currentIndex)

      for (let i = remaining.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1))
        ;[remaining[i], remaining[j]] = [remaining[j], remaining[i]]
      }

      this.playlist = [current, ...remaining]
      this.currentIndex = 0
    },
    restoreOriginalOrder() {
      const currentSong = this.currentSong
      this.playlist = [...this.originalPlaylist]
      this.currentIndex = this.playlist.findIndex((s) => s.id === currentSong?.id)
    },
  },
})

