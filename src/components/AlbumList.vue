<script setup lang="ts">
import { usePlayerStore } from '@/usePlayerStore'
import axios from 'axios'
import { onMounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const route = useRoute()
const albumId = ref(route.params.id)

onMounted(() => {
  fetchAlbum()
  fetchAlbumSongs()
  console.log('Album ID:', albumId.value)
})

const store = usePlayerStore()

const playSong = (song: Song) => {
  const index = songs.value.findIndex((s) => s.id === song.id)
  if (index !== -1) {
    store.setPlaylist(songs.value) // <-- this sets the full album to the global player
    store.playSongByIndex(index)
  }
}

watch(
  () => route.params.id,
  (newId) => {
    albumId.value = newId
    fetchAlbum()
    fetchAlbumSongs()
  }
)

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = seconds % 60
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const songs = ref<Song[]>([])

const fetchAlbumSongs = async () => {
  try {
    const response = await axios.get<Song[]>(`http://localhost:5091/Song/album_id/${albumId.value}`)
    songs.value = response.data

    songs.value.forEach((song, index) => {
      song.index = index + 1
    })
  } catch (error) {
    console.error('Error fetching songs from album:', error)
  }
}

const album = ref<Album>()

const fetchAlbum = async () => {
  try {
    const response = await axios.get<Album>(`http://localhost:5091/SongList/album/${albumId.value}`)
    album.value = response.data
  } catch (error) {
    console.error('Error fetching songs from playlist:', error)
  }
}
</script>

<template>
  <div class="bg-[#362323] rounded-md overflow-hidden">
    <div class="flex align-middle place-items-center p-4">
      <img class="rounded-3xl mr-4" src="../assets/images/album.jpeg" width="250" height="250" />
      <div class="mt-[50px]">
        <div class="text-white text-lg font-arial mb-1">Album</div>
        <div class="text-white text-6xl font-semibold font-arial mb-1">
          {{ album?.name || 'Loading...' }}
        </div>
        <div class="flex align-middle place-items-center mb-2 mt-2">
          <img
            class="rounded-full mr-2.5"
            src='../assets/images/user.jpg'
            width="30"
            height="30"
          />
          <div>
            <div class="text-white text-md font-arial mr-1">Artist,</div>
          </div>
          <div>
            <div class="text-white text-md font-arial">{{ album?.release_date || 'Unknown' }}</div>
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
    <div class="grid grid-cols-7 gap-4 p-2 mx-2 font-arial text-[#753E3E] font-bold">
      <div>#</div>
      <div class="col-span-2">Title</div>
      <div class="col-span-2">Plays</div>
      <div class="col-span-2">Length</div>
    </div>
    <div class="mx-2">
      <mat-divider></mat-divider>
    </div>
    <PerfectScrollbar class="overflow-hidden min-h-[43vh]">
      <button
        v-for="song in songs"
        :key="song.index"
        @click="playSong(song)"
        class="w-full text-left cursor-pointer focus:outline-none grid grid-cols-7 gap-4 p-2 mx-2 font-arial font-bold hover:text-[#888888]"
      >
        <div class="flex place-items-center font-normal">{{ song.index }}</div>
        <div class="col-span-2">
          <div class="flex align-middle place-items-center mb-2 mt-2">
            <div>
              <div class="text-lg font-arial">{{ song.name }}</div>
              <div class="text-xs font-arial font-normal">Artist Name</div>
            </div>
          </div>
        </div>
        <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        <div class="col-span-2 flex place-items-center font-normal">
          {{ formatLength(song.length) }}
        </div>
      </button>
    </PerfectScrollbar>
  </div>
</template>
