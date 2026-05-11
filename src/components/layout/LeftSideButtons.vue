<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore'

const auth = useAuthStore()
</script>

<template>
  <div class="bg-[#362323] rounded-md grid grid-rows-2">
    <RouterLink to="/" class="text-white no-underline ml-1">
      <i class="bi bi-house-fill mr-2 text-[25px]"></i>
      Home
    </RouterLink>
    <RouterLink to="/search" class="text-white no-underline ml-1">
      <i class="bi bi-search mr-2 text-[25px]"></i>
      Search
    </RouterLink>
    <RouterLink to="/local-storage" class="text-white no-underline ml-1">
      <i class="bi bi-device-hdd-fill mr-2 text-[25px]"></i>
      Local Songs
    </RouterLink>
    <RouterLink v-if="auth.isLoggedIn" to="/create-playlist" class="text-white no-underline ml-1">
      <i class="bi bi-plus-lg mr-2 text-[25px]"></i>
      Create Playlist
    </RouterLink>
  </div>
</template>
