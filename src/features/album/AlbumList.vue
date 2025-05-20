<script setup lang="ts">
import { usePlayerStore } from '@/stores/usePlayerStore'
import axios from 'axios'
import { computed, onMounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const route = useRoute()
const albumId = ref(route.params.id)

onMounted(() => {
  fetchAlbum()
  fetchAlbumSongs()
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

    for (const song of response.data) {
      await fetchSongArtists(song.id)
    }
  } catch (error) {
    console.error('Error fetching songs from album:', error)
  }
}

const album = ref<Album>()

const fetchAlbum = async () => {
  try {
    const response = await axios.get<Album>(`http://localhost:5091/Album/${albumId.value}`)
    album.value = response.data
  } catch (error) {
    console.error('Error fetching songs from playlist:', error)
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

const albumArtists = computed(() => {
  const seen = new Set<number>()
  const allArtists: User[] = []

  for (const artistList of Object.values(songArtists.value)) {
    for (const artist of artistList) {
      if (!seen.has(artist.id)) {
        seen.add(artist.id)
        allArtists.push(artist)
      }
    }
  }

  return allArtists
})

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
            <div v-if="albumArtists.length > 0" class="flex flex-wrap gap-x-1">
              <button v-for="(user) in albumArtists" :key="user.id" class="hover:text-[#888888]">
                {{ user.displayName }}<span>,</span>
              </button>
            </div>
            <div v-else>Unknown Artist</div>
          </div>
          <div>
            <div class="text-white text-md font-arial ml-2">{{ album?.releaseDate || 'Unknown' }}</div>
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
        v-for="(song, index) in songs"
        :key="song.id"
        @click="playSong(song)"
        class="w-full text-left cursor-pointer focus:outline-none grid grid-cols-7 gap-4 p-2 mx-2 font-arial font-bold hover:text-[#888888]">
          <div class="flex place-items-center font-normal">{{ index + 1 }}</div>
          <div class="col-span-2">
            <div class="flex align-middle place-items-center mb-2 mt-2">
              <div>
                <div class="text-lg font-arial">{{ song.name }}</div>
                <div class="text-xs font-arial font-normal">
                  <div v-if="songArtists[songs[index]?.id]">
                    <span v-for="(artist, idx) in songArtists[songs[index].id]" :key="artist.id">
                      {{ artist.displayName }}
                      <span v-if="idx < songArtists[songs[index].id].length - 1">, </span>
                    </span>
                  </div>
                  <div v-else>Unknown Artist</div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-span-2 flex place-items-center font-normal">{{ song.listens }}</div>
          <div class="col-span-2 flex place-items-center font-normal">
            {{ formatLength(song.length) }}
          </div>
      </button>
    </PerfectScrollbar>
  </div>
</template>
