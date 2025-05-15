// usePlayerStore.ts
import { defineStore } from 'pinia'

export const usePlayerStore = defineStore('player', {
  state: () => ({
    playlist: [] as Song[],
    currentIndex: 0,
  }),
  getters: {
    currentSong: (state) => state.playlist[state.currentIndex] || null,
  },
  actions: {
    setPlaylist(songs: Song[]) {
      this.playlist = songs
      this.currentIndex = 0
    },
    playSongByIndex(index: number) {
      if (index >= 0 && index < this.playlist.length) {
        this.currentIndex = index
      }
    },
  },
})
