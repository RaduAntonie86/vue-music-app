<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore'
import { usePlayerStore } from '@/stores/usePlayerStore'
import { Album } from '@/types/Album'
import { Playlist } from '@/types/Playlist'
import { Song } from '@/types/Song'
import { User } from '@/types/User'
import { computed, onMounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const route = useRoute()
const albumId = ref(route.params.id)
const authStore = useAuthStore()
const store = usePlayerStore()
const playlists = ref<Playlist[]>([])
const showPlaylistModal = ref(false)
const selectedSong = ref<Song | null>(null)
const album = ref<Album | null>(null)
const songs = ref<Song[]>([])
const songArtists = ref<Record<number, User[]>>({})

const fetchAlbum = async () => {
  try {
    const id = Array.isArray(albumId.value) ? albumId.value[0] : albumId.value
    const numericId = Number(id)
    if (!isNaN(numericId)) {
      album.value = await Album.fetchById(numericId)
    } else {
      console.error('Invalid album ID:', albumId.value)
    }
    if (album.value?.songIds?.length) {
      const songRequests = album.value.songIds.map((id) => Song.fetchById(id))
      const songsArray = await Promise.all(songRequests)
      songs.value = songsArray

      await Promise.all(songs.value.map((song) => fetchSongArtists(song.id)))
    } else {
      songs.value = []
    }
  } catch (error) {
    console.error('Error fetching album or songs:', error)
  }
}

const fetchSongArtists = async (songId: number) => {
  try {
    songArtists.value[songId] = await User.fetchSongArtists(songId)
  } catch (error) {
    console.error(`Error fetching artists for song ${songId}:`, error)
    songArtists.value[songId] = []
  }
}

const albumArtists = computed(() => {
  const seen = new Set<number>()
  const allArtists: User[] = []

  for (const artists of Object.values(songArtists.value)) {
    for (const artist of artists) {
      if (!seen.has(artist.id)) {
        seen.add(artist.id)
        allArtists.push(artist)
      }
    }
  }

  return allArtists
})

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = seconds % 60
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const playSong = (song: Song) => {
  const index = songs.value.findIndex((s) => s.id === song.id)
  if (index !== -1) {
    store.setPlaylist(
      songs.value,
      songs.value.map((s) => songArtists.value[s.id] || []),
      Array(songs.value.length).fill(album.value!)
    )
    store.playSongByIndex(index)
  }
}

const fetchPlaylists = async () => {
  if (authStore.isLoggedIn) {
    try {
      playlists.value = await Playlist.fetchFromUser(authStore.userId!)
    } catch (error) {
      console.error('Error fetching playlists:', error)
    }
  }
}

const openPlaylistModal = (song: Song) => {
  selectedSong.value = song
  showPlaylistModal.value = true
}

const closePlaylistModal = () => {
  selectedSong.value = null
  showPlaylistModal.value = false
}

const addSongToPlaylist = async (playlistId: number) => {
  if (!selectedSong.value) return
  try {
    Playlist.addSong(playlistId, selectedSong.value.id)
    console.log(`Added song ${selectedSong.value.id} to playlist ${playlistId}`)
    closePlaylistModal()
  } catch (error) {
    console.error('Error adding song to playlist:', error)
  }
}

const artistImageSource = computed(() => {
  const firstArtist = albumArtists.value[0]
  const path = firstArtist?.imagePath?.trim()

  if (!path) return '/images/users/user.jpg'

  if (path.startsWith('/') || path.includes('images/')) {
    return path
  }

  return `/images/users/${path}`
})

watch(
  () => route.params.id,
  (newId) => {
    albumId.value = newId
    fetchAlbum()
  }
)

onMounted(() => {
  fetchPlaylists()
  fetchAlbum()
})
</script>

<template>
  <div class="bg-[#362323] rounded-md overflow-hidden">
    <div class="flex align-middle place-items-center p-4">
      <img
        class="rounded-3xl mr-4"
        :src="album?.imagePath?.trim() ? album.imagePath : '/images/albums/album.jpeg'"
        width="250"
        height="250"
      />
      <div class="mt-[50px]">
        <div class="text-white text-lg font-arial mb-1">Album</div>
        <div class="text-white text-6xl font-semibold font-arial mb-1">
          {{ album?.name || 'Loading...' }}
        </div>
        <div class="flex align-middle place-items-center mb-2 mt-2">
        <img
          class="rounded-full mr-2.5 aspect-square object-cover w-[30px] h-[30px]"
          :src="artistImageSource"
          @error="(e) => {
            const img = e.target as HTMLImageElement
            if (!img.src.endsWith('/user.jpg')) {
              img.src = '/images/users/user.jpg'
            }
          }"
        />
          <div>
            <div v-if="albumArtists.length > 0" class="flex flex-wrap gap-x-1">
              <button v-for="user in albumArtists" :key="user.id" class="hover:text-[#888888]">
                {{ user.displayName
                }}<span v-if="user !== albumArtists[albumArtists.length - 1]">, </span>
              </button>
            </div>
            <div v-else>Unknown Artist</div>
          </div>
          <div class="text-white text-md font-arial ml-2">
            {{ album?.releaseDate || 'Unknown' }}
          </div>
        </div>
      </div>
    </div>

    <div class="mx-2">
      <div class="flex justify-between items-center">
        <div
          class="rounded-full ml-1 bg-[#753E3E] size-12 flex place-content-center place-items-center"
        >
          <i class="fas fa-play text-2xl"></i>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-9 gap-4 p-2 mx-2 font-arial text-[#753E3E] font-bold">
      <div>#</div>
      <div class="col-span-2">Title</div>
      <div class="col-span-2">Plays</div>
      <div class="col-span-2">Length</div>
      <div class="col-span-2"></div>
    </div>

    <div class="mx-2">
      <custom-divider
        :width="'100%'"
        :height="9"
      />
    </div>

    <PerfectScrollbar class="overflow-hidden min-h-[43vh] max-h-[43vh]">
      <div
        v-for="(song, index) in songs"
        :key="song.id"
        class="w-full text-left cursor-pointer grid grid-cols-9 gap-4 p-2 mx-2 font-arial font-bold hover:bg-[#444444]"
      >
        <div @click="playSong(song)" class="flex place-items-center font-normal">
          {{ index + 1 }}
        </div>
        <div @click="playSong(song)" class="col-span-2">
          <div class="flex align-middle place-items-center mb-2 mt-2">
            <div>
              <div class="text-lg font-arial">{{ song.name }}</div>
              <div class="text-xs font-arial font-normal">
                <div v-if="songArtists[song.id]?.length">
                  <span v-for="(artist, idx) in songArtists[song.id]" :key="artist.id">
                    {{ artist.displayName }}
                    <span v-if="idx < songArtists[song.id].length - 1">, </span>
                  </span>
                </div>
                <div v-else>Unknown Artist</div>
              </div>
            </div>
          </div>
        </div>
        <div @click="playSong(song)" class="col-span-2 flex place-items-center font-normal">
          {{ song.listens }}
        </div>
        <div @click="playSong(song)" class="col-span-2 flex place-items-center font-normal">
          {{ formatLength(song.length) }}
        </div>
        <div class="col-span-2 flex place-items-center font-normal hover:text-xl">
          <i
            @click.stop="openPlaylistModal(song)"
            class="bi bi-plus-lg hover:text-white cursor-pointer"
          ></i>
        </div>
      </div>
    </PerfectScrollbar>

    <div
      v-if="showPlaylistModal"
      class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50"
    >
      <div class="bg-[#2e2e2e] rounded-xl p-6 w-[400px] max-h-[80vh] overflow-y-auto shadow-2xl">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-white text-xl font-bold">Add to Playlist</h2>
          <button @click="closePlaylistModal" class="text-white text-2xl">&times;</button>
        </div>
        <div v-if="authStore.isLoggedIn">
          <PerfectScrollbar v-if="playlists.length > 0" class="overflow-hidden max-h-[43vh]">
            <div
              v-for="playlist in playlists"
              :key="playlist.id"
              @click="addSongToPlaylist(playlist.id!)"
              class="p-3 rounded-md hover:bg-[#444] text-white cursor-pointer flex items-center gap-3"
            >
              <img
                :src="playlist.imagePath || '/images/albums/album.jpeg'"
                class="w-10 h-10 object-cover rounded-md"
              />
              <div>
                <div class="font-semibold">{{ playlist.name }}</div>
                <div class="text-sm text-gray-400">{{ playlist.description }}</div>
              </div>
            </div>
          </PerfectScrollbar>
          <div v-else class="text-gray-400">No playlists found.</div>
        </div>
        <div v-else class="text-gray-400">Please log in.</div>
      </div>
    </div>
  </div>
</template>
