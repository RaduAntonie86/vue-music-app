import type { Album } from '@/types/Album'
import type { PlaylistItem } from '@/types/PlaylistItem'
import type { Song } from '@/types/Song'
import type { User } from '@/types/User'
import { defineStore } from 'pinia'

export const usePlayerStore = defineStore('player', {
  state: () => ({
    playlist: [] as Song[],
    artists: [] as User[][],
    originalPlaylist: [] as Song[],
    originalArtists: [] as User[][],
    albums: [] as Album[],
    originalAlbums: [] as Album[],
    currentIndex: 0
  }),
  getters: {
    currentSong: (state) => state.playlist[state.currentIndex] || null,
    currentArtists: (state) => state.artists[state.currentIndex] || [],
    currentAlbum: (state) => state.albums[state.currentIndex] || null
  },
  actions: {
    setPlaylist(songs: Song[], artists: User[][], albums: Album[]) {
      this.playlist = songs
      this.artists = artists
      this.albums = albums

      this.originalPlaylist = [...songs]
      this.originalArtists = artists.map((a) => [...a])
      this.originalAlbums = [...albums]

      this.currentIndex = 0
    },
    playSongByIndex(index: number) {
      if (index >= 0 && index < this.playlist.length) {
        this.currentIndex = index
      }
    },
    shufflePlaylist() {
      const combined: PlaylistItem[] = this.playlist.map((song, i) => ({
        song,
        artistGroup: this.artists[i],
        album: this.albums[i],
      }))

      const shuffled = shuffleArray(combined, this.currentIndex)

      this.playlist = shuffled.map((s) => s.song)
      this.artists = shuffled.map((s) => s.artistGroup)
      this.albums = shuffled.map((s) => s.album)
      this.currentIndex = 0
    },
    restoreOriginalOrder() {
      const currentSong = this.currentSong
      this.playlist = [...this.originalPlaylist]
      this.artists = this.originalArtists.map((a) => [...a])
      this.albums = [...this.originalAlbums]
      this.currentIndex = this.playlist.findIndex((s) => s.id === currentSong?.id)
    }
  }
})
function shuffleArray(arr: PlaylistItem[], currentIndex: number) {
  const currentItem = arr[currentIndex]
  const remaining = arr.filter((_, i) => i !== currentIndex)

  for (let i = remaining.length - 1; i > 0; i--) {
    const j = Math.floor(Math.random() * (i + 1))
    ;[remaining[i], remaining[j]] = [remaining[j], remaining[i]]
  }

  return [currentItem, ...remaining]
}

