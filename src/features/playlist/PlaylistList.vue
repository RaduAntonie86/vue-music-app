<script setup lang="ts">
import { ref, watch, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import { usePlayerStore } from '@/stores/usePlayerStore'
import { Playlist } from '@/types/Playlist'
import { Song } from '@/types/Song'
import { User } from '@/types/User'
import { Album } from '@/types/Album'
import router from '@/router'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const route = useRoute()
const playlistId = ref(Number(route.params.id))
const playlist = ref<Playlist | null>(null)
const songs = ref<Song[]>([])
const users = ref<User[]>([])
const albums = ref<Record<number, Album>>({})
const songArtists = ref<Record<number, User[]>>({})
const authStore = useAuthStore()
const store = usePlayerStore()
const selectedSong = ref<Song | null>(null)
const showPlaylistModal = ref(false)
const userPlaylists = ref<Playlist[]>([])

async function fetchPlaylist() {
  if (!playlistId.value) return
  playlist.value = await Playlist.fetchById(playlistId.value)
  console.log(playlist.value)
  if (playlist.value) {
    if (playlist.value.songIds?.length) {
      await fetchSongsByIds(playlist.value.songIds)
    }
    if (playlist.value.userIds?.length) {
      await fetchUsersByIds(playlist.value.userIds)
    }
  }
}

const fetchSongsByIds = async (ids: number[]) => {
  try {
    const fetchedSongs = await Song.fetchByIds(ids)
    if (!fetchedSongs) return
    songs.value = fetchedSongs

    for (const song of songs.value) {
      const artists = await User.fetchSongArtists(song.id)
      songArtists.value[song.id] = artists || []
    }

    const albumsMap = await Album.fetchAlbumsForSongs(songs.value.map((song) => song.id))
    if (albumsMap) {
      albums.value = albumsMap
    }
  } catch (error) {
    console.error('Error fetching songs:', error)
  }
}

const fetchUsersByIds = async (ids: number[]) => {
  try {
    const fetchedUsers = await User.fetchUsersByIds(ids)
    if (fetchedUsers) users.value = fetchedUsers
  } catch (error) {
    console.error('Error fetching users:', error)
  }
}

const playSong = (song: Song) => {
  const index = songs.value.findIndex((s) => s.id === song.id)
  if (index !== -1) {
    const songsWithBlobs = songs.value.map((s) => ({
      ...s,
      audioPath: localFilesMap.value[s.id] || s.path,
      isLocal: !!localFilesMap.value[s.id],
    }))

    store.setPlaylist(
      songsWithBlobs,
      songsWithBlobs.map((song) => songArtists.value[song.id] || []),
      songsWithBlobs.map((song) => albums.value[song.id] || { id: -1, name: 'Local', imagePath: '' })
    )

    store.playSongByIndex(index)
  }
}

const deletePlaylist = async () => {
  if (isOwner.value) {
    try {
      const confirmed = confirm('Are you sure you want to delete this playlist?')
      if (!confirmed) return
      Playlist.deletePlaylist(playlistId.value)
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

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = seconds % 60
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const playlistSongIds = computed(() => playlist.value?.songIds || [])
const playlistUserIds = computed(() => playlist.value?.userIds || [])
const songsById = computed(() => {
  const map = new Map<number, Song>()
  songs.value.forEach((song) => map.set(song.id, song))
  return map
})
const usersById = computed(() => {
  const map = new Map<number, User>()
  users.value.forEach((user) => map.set(user.id, user))
  return map
})
const firstUser = computed(() => {
  if (playlistUserIds.value.length === 0) return null
  return usersById.value.get(playlistUserIds.value[0]) || null
})
const isOwner = computed(() => {
  return users.value.some((user) => user.id === authStore.userId)
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
    Playlist.addSong(playlistId, selectedSong.value.id)
    console.log(`Added song ${selectedSong.value.id} to playlist ${playlistId}`)
    closePlaylistModal()
  } catch (error) {
    console.error('Error adding song to playlist:', error)
  }
}

const fetchUserPlaylists = async () => {
  if (!authStore.userId) return
  try {
    userPlaylists.value = await Playlist.fetchFromUser(authStore.userId)
    userPlaylists.value = userPlaylists.value.filter((p) => p.id !== playlist.value?.id)
  } catch (error) {
    console.error('Error fetching user playlists:', error)
  }
}

const userImageSource = computed(() => {
  const path = firstUser.value?.imagePath?.trim()

  if (!path) return '/images/users/user.jpg'

  if (path.startsWith('/') || path.includes('images/')) {
    return path
  }

  return `/images/users/${path}`
})

const localSongs = ref<Song[]>([])
const localFilesMap = ref<Record<number, string>>({}) // id -> Blob URL
let localIdCounter = 1_000_000 // local song IDs

const addLocalSongs = async (files: FileList | null) => {
  if (!files) return

  for (const file of Array.from(files)) {
    const id = localIdCounter++
    const url = URL.createObjectURL(file)

    localFilesMap.value[id] = url
    localSongs.value.push({
      id,
      name: file.name.replace(/\.[^/.]+$/, ''),
      length: 0, // Could be extracted from metadata if needed
      listens: 0,
      path: '', // You could store this temporarily if needed
    } as Song)

    playlist.value?.songIds.push(id)
    songs.value.push(localSongs.value[localSongs.value.length - 1])
  }
}

watch(
  () => route.params.id,
  (newId) => {
    playlistId.value = Number(newId)
    fetchPlaylist()
  }
)

onMounted(() => {
  fetchPlaylist()
  fetchUserPlaylists()
})
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
          :src="userImageSource"
          @error="(e) => {
            const img = e.target as HTMLImageElement
            if (!img.src.endsWith('/user.jpg')) {
              img.src = '/images/users/user.jpg'
            }
          }"
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
        <label
          v-if="isOwner"
          for="fileInput"
          class="cursor-pointer font-arial text-xl font-semibold bg-white rounded-xl px-4 py-2 text-[#882323] hover:text-[#888888]"
        >
          Add Local Songs
        </label>
        <input
          v-if="isOwner"
          id="fileInput"
          type="file"
          accept="audio/*"
          multiple
          class="hidden"
          @change="(e) => addLocalSongs((e.target as HTMLInputElement).files)"
        />
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
      <custom-divider
        :width="'100%'"
        :height="9"
      />
    </div>
    <PerfectScrollbar class="min-h-[38vh] max-h-[38vh] overflow-hidden">
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
