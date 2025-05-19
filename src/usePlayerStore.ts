import { defineStore } from 'pinia'

export const usePlayerStore = defineStore('player', {
  state: () => ({
    playlist: [] as Song[],
    artists: [] as User[][],
    originalPlaylist: [] as Song[],
    originalArtists: [] as User[][],
    currentIndex: 0,
  }),
  getters: {
    currentSong: (state) => state.playlist[state.currentIndex] || null,
    currentArtists: (state) => state.artists[state.currentIndex] || [],
  },
  actions: {
    setPlaylist(songs: Song[], artists: User[][]) {
      this.playlist = songs
      this.artists = artists
      this.originalPlaylist = [...songs]
      this.originalArtists = artists.map(a => [...a])
      this.currentIndex = 0
    },
    playSongByIndex(index: number) {
      if (index >= 0 && index < this.playlist.length) {
        this.currentIndex = index
      }
    },
    shufflePlaylist() {
      const currentSong = this.playlist[this.currentIndex]
      const currentArtists = this.artists[this.currentIndex]

      const combined = this.playlist.map((song, i) => ({
        song,
        artistGroup: this.artists[i],
      }))

      const remaining = combined.filter((_, i) => i !== this.currentIndex)

      for (let i = remaining.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1))
        ;[remaining[i], remaining[j]] = [remaining[j], remaining[i]]
      }

      const shuffled = [{ song: currentSong, artistGroup: currentArtists }, ...remaining]

      this.playlist = shuffled.map(s => s.song)
      this.artists = shuffled.map(s => s.artistGroup)
      this.currentIndex = 0
    },
    restoreOriginalOrder() {
      const currentSong = this.currentSong
      this.playlist = [...this.originalPlaylist]
      this.artists = this.originalArtists.map(a => [...a])
      this.currentIndex = this.playlist.findIndex(s => s.id === currentSong?.id)
    },
  },
})