<script setup lang="ts">
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import { Playlist } from '@/types/Playlist'
import { onMounted, ref } from 'vue'

const existingPlaylistIds = ref<number[]>([])
const playlist_name = ref('')
const playlist_description = ref('')
const auth = useAuthStore()

const fetchPlaylists = async () => {
  try {
    const playlists = await Playlist.fetchAll()
    if (!playlists) {
      existingPlaylistIds.value = []
      return
    }
    existingPlaylistIds.value = playlists
      .map((p) => p.id)
      .filter((id): id is number => id !== undefined)
  } catch (error) {
    console.error('Error fetching playlists:', error)
  }
}

const createPlaylist = async () => {
  if (auth.userId == null) {
    console.log('Could not find user Id.')
    alert('You are not logged in.')
    router.push('/')
    return
  }

  try {
    const payload = {
      name: playlist_name.value,
      description: playlist_description.value,
      imagePath: '',
      userIds: [auth.userId],
      songIds: [],
    } as Playlist
    Playlist.createPlaylist(payload)
    console.log('Playlist created successfully')
    router.push('/')
  } catch (error) {
    console.error('Error creating playlist:', error)
  }
}

onMounted(() => {
  fetchPlaylists()
})
</script>

<template>
  <div class="bg-[#362323] rounded-md">
    <div>
      <div
        class="flex align-middle place-items-center p-4 text-6xl font-semibold font-arial mb-3 text-white"
      >
        Create Playlist
      </div>
      <input
        v-model="playlist_name"
        class="form-control me-2 input-rounded text-[#efd0d0] bg-custom max-w-[30vh] ml-8 mb-5"
        type="text"
        placeholder="Playlist Name"
      />
      <input
        v-model="playlist_description"
        class="form-control me-2 input-rounded text-[#efd0d0] bg-custom max-w-[30vh] ml-8 mb-4"
        type="text"
        placeholder="Playlist Description"
      />
      <button
        @click="createPlaylist"
        class="font-arial text-xl font-semibold bg-white rounded-xl ml-8 px-4 py-2 hover:text-[#888888] text-[#882323] mb-4"
      >
        Create
      </button>
    </div>
  </div>
</template>
