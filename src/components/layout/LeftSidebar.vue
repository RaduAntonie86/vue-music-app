<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'
import { onMounted, ref, watch } from 'vue'
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import { Playlist } from '@/types/Playlist'

const playlists = ref<Playlist[]>([])
const authStore = useAuthStore()

const fetchPlaylists = async () => {
  console.log('AuthStore userId:', authStore.userId)
  try {
    playlists.value = await Playlist.fetchFromUser(authStore.userId!)
  } catch (error) {
    console.error('Error fetching playlists:', error)
  }
}

const handlePlaylistClick = (id: number) => {
  console.log('Clicked playlist ID:', id)
  router.push({ name: 'playlist', params: { id: id.toString() } })
}

onMounted(() => {
  fetchPlaylists()
})

watch(
  () => authStore.userId,
  (newId) => {
    if (newId) {
      fetchPlaylists()
    }
  },
  { immediate: true } // triggers the watcher right away on mount
)
</script>

<style>
@import 'vue3-perfect-scrollbar/style.css';
</style>

<template>
  <PerfectScrollbar class="bg-[#362323] max-h-[75vh] rounded-md p-1 overflow-auto">
    <div
      v-for="playlist in playlists"
      :key="playlist.id"
      @click="handlePlaylistClick(playlist.id!)"
    >
      <playlist-preview
        :playlistName="playlist.name"
        :description="playlist.description"
        :imagePath="playlist.imagePath ? playlist.imagePath : '/images/albums/album.jpeg'"
      >
      </playlist-preview>
    </div>
  </PerfectScrollbar>
</template>
