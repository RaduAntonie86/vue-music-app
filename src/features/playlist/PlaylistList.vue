<script setup lang="ts">
import { usePlayerStore } from '@/stores/usePlayerStore'
import axios from 'axios'
import { onMounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const route = useRoute()
const playlistId = ref(route.params.id)

onMounted(() => {
  fetchPlaylist()
  fetchPlaylistSongs()
  fetchPlaylistAlbums()
  fetchPlaylistUsers()
})

const store = usePlayerStore()

const playSong = (song: Song) => {
  const index = songs.value.findIndex((s) => s.id === song.id)
  if (index !== -1) {
    store.setPlaylist(
      songs.value,
      songs.value.map(song => songArtists.value[song.id] || [])
    )
    store.playSongByIndex(index)
  }
}

watch(
  () => route.params.id,
  (newId) => {
    playlistId.value = newId
    fetchPlaylist()
    fetchPlaylistSongs()
    fetchPlaylistAlbums()
    fetchPlaylistUsers()
  }
)

const playlist = ref<Playlist>()

const fetchPlaylist = async () => {
  try {
    const response = await axios.get<Playlist>(
      `http://localhost:5091/Playlist/${playlistId.value}`
    )
    playlist.value = response.data
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

const fetchPlaylistSongs = async () => {
  try {
    const response = await axios.get<Song[]>(
      `http://localhost:5091/Song/playlist_id/${playlistId.value}`
    )
    songs.value = response.data

    for (const song of response.data) {
      await fetchSongArtists(song.id)
    }
  } catch (error) {
    console.error('Error fetching songs from playlist:', error)
  }
}

const users = ref<User[]>([])

const fetchPlaylistUsers = async () => {
  try {
    const response = await axios.get<User[]>(
      `http://localhost:5091/User/playlist_id/${playlistId.value}`
    )
    users.value = response.data
  } catch (error) {
    console.error('Error fetching users from playlist:', error)
  }
}

const albums = ref<Album[]>([])

const fetchPlaylistAlbums = async () => {
  try {
    const response = await axios.get<Album[]>(
      `http://localhost:5091/Album/playlist_id/${playlistId.value}`
    )
    albums.value = response.data
  } catch (error) {
    console.error('Error fetching users from playlist:', error)
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
</script>

<template>
  <div class="bg-[#362323] rounded-md">
    <div class="flex align-middle place-items-center p-4">
      <img class="rounded-3xl mr-4" src="../../assets/images/album.jpeg" width="250" height="250" />
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
          <img class="rounded-full mr-2.5" src="../../assets/images/user.jpg" width="30" height="30" />
          <div class="flex items-center text-white text-md font-arial">
            <div v-if="users.length > 0" class="flex flex-wrap gap-x-1">
              <div class="hover:text-[#888888]" v-for="(user, index) in users" :key="index">
                <button>
                  {{ user.displayName }}<span v-if="index < users.length - 1">,</span>
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
      </div>
    </div>
    <div class="grid grid-cols-7 p-2 mx-2 font-arial text-[#753E3E] font-bold">
      <div>#</div>
      <div class="col-span-2">Title</div>
      <div class="col-span-2">Album</div>
      <div class="col-span-2">Length</div>
    </div>
    <div class="mx-2">
      <mat-divider></mat-divider>
    </div>
    <PerfectScrollbar class="min-h-[40vh] max-h-[60vh] overflow-hidden">
      <button
        v-for="(song, index) in songs"
        :key="song.id"
        @click="playSong(song)"
        class="w-full text-left cursor-pointer focus:outline-none hover:text-[#888888]"
      >
        <div class="grid grid-cols-7 p-2 mx-2 font-arial font-bold">
          <div class="flex place-items-center font-normal">{{ index + 1 }}</div>
          <div class="col-span-2">
            <div class="flex align-middle place-items-center mb-2 mt-2">
              <img
                class="rounded-lg mr-2.5"
                src="../../assets/images/album.jpeg"
                width="50"
                height="50"
              />
              <div>
                <div class="text-lg font-arial">{{ song.name }}</div>
                <div class="text-xs font-arial font-normal">
                  <div v-if="songArtists[songs[index]?.id]">
                    <span v-for="(artist, idx) in songArtists[songs[index].id]" :key="artist.id">
                      {{ artist.displayName
                      }}<span v-if="idx < songArtists[songs[index].id].length - 1">, </span>
                    </span>
                  </div>
                  <div v-else>Unknown Artist</div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-span-2 flex place-items-center font-normal">{{ albums[index].name }}</div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ formatLength(song.length) }}
          </div>
        </div>
      </button>
    </PerfectScrollbar>
  </div>
</template>
