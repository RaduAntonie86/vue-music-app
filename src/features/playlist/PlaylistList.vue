<script setup lang="ts">
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import { usePlayerStore } from '@/stores/usePlayerStore'
import axios from 'axios'
import { computed, onMounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const route = useRoute()
const playlistId = ref(route.params.id)
const auth = useAuthStore()
const selectedSong = ref<Song | null>(null)
const showPlaylistModal = ref(false)
const authStore = useAuthStore()

onMounted(() => {
  fetchPlaylist()
  fetchUserPlaylists()
})

const store = usePlayerStore()

const playSong = (song: Song) => {
  const index = songs.value.findIndex((s) => s.id === song.id)
  if (index !== -1) {
    store.setPlaylist(
      songs.value,
      songs.value.map((song) => songArtists.value[song.id] || []),
      songs.value.map((song) => songToAlbumMap.value.get(song.id)!)
    )
    store.playSongByIndex(index)
  }
}

watch(
  () => route.params.id,
  (newId) => {
    playlistId.value = newId
    fetchPlaylist()
    if (playlist.value?.imagePath !== undefined && playlist.value?.imagePath !== null)
      console.log(playlist.value?.imagePath)
  }
)

const playlist = ref<Playlist>()

const fetchPlaylist = async () => {
  try {
    const response = await axios.get<Playlist>(`http://localhost:5091/Playlist/${playlistId.value}`)
    playlist.value = response.data

    // Print songIds and userIds if they exist
    if (playlist.value?.songIds) {
      await fetchSongsByIds(playlist.value.songIds)
      console.log('Song IDs:', playlist.value.songIds)
    }

    if (playlist.value?.userIds) {
      await fetchUsersByIds(playlist.value.userIds)
      console.log('User IDs:', playlist.value.userIds)
    }
  } catch (error) {
    console.error('Error fetching playlist:', error)
  }
}

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = seconds % 60
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const songs = ref<Song[]>([])

const fetchSongsByIds = async (ids: number[]) => {
  try {
    const promises = ids.map((id) => axios.get<Song>(`http://localhost:5091/Song/${id}`))
    const responses = await Promise.all(promises)
    songs.value = responses.map((res) => res.data)

    for (const song of songs.value) {
      await fetchSongArtists(song.id)
    }

    await fetchAlbumsForSongs(songs.value.map((song) => song.id))
  } catch (error) {
    console.error('Error fetching songs:', error)
  }
}

const users = ref<User[]>([])

const fetchUsersByIds = async (ids: number[]) => {
  try {
    const promises = ids.map((id) => axios.get<User>(`http://localhost:5091/User/${id}`))
    const responses = await Promise.all(promises)
    users.value = responses.map((res) => res.data)
    console.log('Fetched users:', users.value) // <--- check what you get here
  } catch (error) {
    console.error('Error fetching users:', error)
  }
}

const albums = ref<Record<number, Album>>({})

const fetchAlbumsForSongs = async (songIds: number[]) => {
  try {
    const promises = songIds.map((id) =>
      axios.get<Album>(`http://localhost:5091/Album/song_id/${id}`)
    )
    const responses = await Promise.all(promises)
    responses.forEach((res, idx) => {
      albums.value[songIds[idx]] = res.data
    })
  } catch (error) {
    console.error('Error fetching albums:', error)
  }
}

const songArtists = ref<Record<number, User[]>>({})

const fetchSongArtists = async (songId: number) => {
  try {
    const response = await axios.get<User[]>(`http://localhost:5091/User/song_id/${songId}`)
    songArtists.value[songId] = response.data
  } catch (error) {
    console.error(`Error fetching artists for album ${songId}:`, error)
  }
}

const deletePlaylist = async () => {
  if (isOwner.value) {
    try {
      const confirmed = confirm('Are you sure you want to delete this playlist?')
      if (!confirmed) return

      await axios.delete(`http://localhost:5091/Playlist/${playlistId.value}`)
      alert('Playlist deleted successfully.')
      router.push('/')
    } catch (error) {
      console.error('Error deleting playlist:', error)
      alert('Failed to delete the playlist.')
    }
  } else {
    alert('You are not allowed to do this.')
  }
}

const isOwner = computed(() => {
  return users.value.some((user) => user.id === auth.userId)
})

const songToAlbumMap = computed(() => {
  const map = new Map<number, Album>()
  for (const songId in albums.value) {
    map.set(Number(songId), albums.value[songId])
  }
  return map
})

const playlistSongIds = computed(() => playlist.value?.songIds || [])
const playlistUserIds = computed(() => playlist.value?.userIds || [])

// Map songIds to song objects (or empty array if not loaded)
const songsById = computed(() => {
  const map = new Map<number, Song>()
  songs.value.forEach((song) => map.set(song.id, song))
  return map
})

const usersById = computed(() => {
  const map = new Map<number, User>()
  users.value.forEach((user) => {
    map.set(Number(user.id), user) // convert key to number explicitly
  })
  return map
})

const firstUser = computed(() => {
  if (playlistUserIds.value.length === 0) return null
  return usersById.value.get(playlistUserIds.value[0]) || null
})

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
    await axios.post(
      `http://localhost:5091/Playlist/${playlistId}/addSong/${selectedSong.value.id}`
    )
    console.log(`Added song ${selectedSong.value.id} to playlist ${playlistId}`)
    closePlaylistModal()
  } catch (error) {
    console.error('Error adding song to playlist:', error)
  }
}

const userPlaylists = ref<Playlist[]>([])

const fetchUserPlaylists = async () => {
  if (!auth.userId) return
  try {
    const response = await axios.get<Playlist[]>(
      `http://localhost:5091/Playlist/user/${auth.userId}`
    )
    // Exclude current playlist from the list
    userPlaylists.value = response.data.filter((p) => p.id !== playlist.value?.id)
  } catch (error) {
    console.error('Error fetching user playlists:', error)
  }
}
</script>

<template>
  <div class="bg-[#362323] rounded-md">
    <div class="flex align-middle place-items-center p-4">
      <img
        class="rounded-3xl mr-4"
        :src="
          playlist?.imagePath && playlist.imagePath.trim() !== ''
            ? playlist.imagePath
            : '/images/albums/album.jpeg'
        "
        width="250"
        height="250"
      />
      <div class="mt-[50px]">
        <div class="text-white text-lg font-arial mb-1">Playlist</div>
        <div class="text-white text-6xl font-semibold font-arial mb-1">
          {{ playlist?.name || 'Loading...' }}
        </div>
        <div class="text-white text-2xl font-arial mb-2">
          {{ playlist?.description || 'No description.' }}
        </div>
        <div class="text-white text-lg font-arial">Made by:</div>
        <div class="flex align-middle place-items-center mb-2 mt-2">
          <img
            class="rounded-full mr-2.5 aspect-square object-cover w-[30px] h-[30px]"
            :src="
              firstUser && firstUser.imagePath?.trim() !== ''
                ? firstUser.imagePath
                : '/images/users/user.jpg'
            "
            @error="
              (e) => {
                const img = e.target as HTMLImageElement
                if (!img.src.endsWith('/user.jpg')) {
                  img.src = '/images/users/user.jpg'
                }
              }
            "
          />
          <div class="flex items-center text-white text-md font-arial">
            <div
              v-if="playlistUserIds.length > 0 && usersById.size > 0"
              class="flex flex-wrap gap-x-1"
            >
              <div
                class="hover:text-[#888888]"
                v-for="(userId, index) in playlistUserIds"
                :key="userId"
              >
                <button>
                  {{ usersById.get(userId)?.displayName || 'Unknown user'
                  }}<span v-if="index < playlistUserIds.length - 1">,</span>
                </button>
              </div>
            </div>
            <div v-else>Invalid user.</div>
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
        <button
          v-if="isOwner"
          @click="deletePlaylist()"
          class="font-arial text-xl font-semibold bg-white rounded-xl ml-8 px-4 py-2 hover:text-[#888888] text-[#882323] mb-4"
        >
          Delete Playlist
        </button>
      </div>
    </div>
    <div class="grid grid-cols-9 p-2 mx-2 font-arial text-[#753E3E] font-bold">
      <div>#</div>
      <div class="col-span-2">Title</div>
      <div class="col-span-2">Album</div>
      <div class="col-span-2">Length</div>
      <div class="col-span-2"></div>
    </div>
    <div class="mx-2">
      <mat-divider></mat-divider>
    </div>
    <PerfectScrollbar class="min-h-[40vh] max-h-[20vh] overflow-hidden">
      <button
        v-for="(songId, index) in playlistSongIds"
        :key="songId"
        @click="
          () => {
            const song = songsById.get(songId)
            if (song) playSong(song)
          }
        "
        class="w-full text-left cursor-pointer focus:outline-none hover:text-[#888888]"
      >
        <div class="grid grid-cols-9 p-2 mx-2 font-arial font-bold">
          <div class="flex place-items-center font-normal">{{ index + 1 }}</div>
          <div class="col-span-2">
            <div class="flex align-middle place-items-center mb-2 mt-2">
              <img
                class="rounded-lg mr-2.5"
                :src="
                  albums[songId]?.imagePath?.trim() !== ''
                    ? albums[songId].imagePath
                    : 'images/albums/album.jpeg'
                "
                width="50"
                height="50"
              />
              <div>
                <div class="text-lg font-arial">
                  {{ songsById.get(songId)?.name || 'Unknown song' }}
                </div>
                <div class="text-xs font-arial font-normal">
                  <div v-if="songArtists[songId] && songArtists[songId].length > 0">
                    <span v-for="(artist, idx) in songArtists[songId]" :key="artist.id">
                      {{ artist.displayName
                      }}<span v-if="idx < songArtists[songId].length - 1">, </span>
                    </span>
                  </div>
                  <div v-else>Unknown Artist</div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ albums[songId]?.name || 'Unknown Album' }}
          </div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ formatLength(songsById.get(songId)?.length || 0) }}
          </div>
          <div class="col-span-2 flex place-items-center font-normal hover:text-xl">
            <i
              @click.stop="
                () => {
                  const song = songsById.get(songId)
                  if (song) openPlaylistModal(song)
                }
              "
              class="bi bi-plus-lg hover:text-white cursor-pointer"
            ></i>
          </div>
        </div>
      </button>
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
          <PerfectScrollbar v-if="userPlaylists.length > 0" class="overflow-hidden max-h-[43vh]">
            <div
              v-for="playlist in userPlaylists"
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
