<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore'
import { Playlist } from '@/types/Playlist'
import { Song } from '@/types/Song'
import { computed, onMounted, ref } from 'vue'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const props = defineProps<{
  song: Song | null
  visible: boolean
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'added', playlistId: number): void
}>()

const authStore = useAuthStore()
const playlists = ref<Playlist[]>([])

const songId = computed(() => props.song?.id ?? null)

const fetchPlaylists = async () => {
  if (authStore.isLoggedIn) {
    try {
      playlists.value = await Playlist.fetchFromUser(authStore.userId!)
    } catch (error) {
      console.error('Error fetching playlists:', error)
    }
  }
}

const addSongToPlaylist = async (playlistId: number) => {
  if (!props.song) return
  try {
    await Playlist.addSong(playlistId, props.song.id)
    emit('added', playlistId)
    emit('close')
  } catch (error) {
    console.error('Error adding song to playlist:', error)
  }
}

onMounted(fetchPlaylists)
</script>

<template>
  <Teleport to="body">
    <div
      v-if="visible"
      class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50"
      @click.self="emit('close')"
    >
      <div class="bg-[#2e2e2e] rounded-xl p-6 w-[400px] max-h-[80vh] overflow-y-auto shadow-2xl">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-white text-xl font-bold">Add to Playlist</h2>
          <button @click="emit('close')" class="text-white text-2xl">&times;</button>
        </div>
        <div v-if="authStore.isLoggedIn">
          <PerfectScrollbar v-if="playlists.length > 0" class="overflow-hidden max-h-[43vh]">
            <div
              v-for="playlist in playlists"
              :key="playlist.id"
              :class="[
                'text-white items-center gap-3 p-3 rounded-md flex',
                songId !== null && !playlist.songIds.includes(songId)
                  ? 'hover:bg-[#444] cursor-pointer'
                  : 'opacity-50 cursor-default'
              ]"
              @click="
                songId !== null && !playlist.songIds.includes(songId)
                  ? addSongToPlaylist(playlist.id!)
                  : undefined
              "
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
  </Teleport>
</template>
