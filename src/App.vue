<script setup lang="ts">
import { computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import MusicPlayer from './components/layout/MusicPlayer.vue'

const authStore = useAuthStore()
const route = useRoute()
const router = useRouter()

const hidePlayerRoutes = ['/login', '/signup']

const showPlayer = computed(() => {
  return route?.path && !hidePlayerRoutes.includes(route.path)
})

const handleLogout = () => {
  authStore.clearAll()
  router.push('/login')
}

</script>

<template>
  <div class="bg-[#21201e] flex flex-col h-screen">
    <div class="grid grid-cols-2">
      <RouterLink
        to="/"
        class="font-arial text-white no-underline mb-3 text-5xl font-semibold text-left"
        >App Name
      </RouterLink>
      <div class="text-right">
        <RouterLink
          v-if="!authStore.isLoggedIn"
          to="/login"
          class="font-arial text-white no-underline mb-3 text-5xl font-semibold"
        >Log In
        </RouterLink>
        <button
          v-else
          @click="handleLogout"
          class="font-arial text-white no-underline mb-3 text-5xl font-semibold"
        >Log Out
        </button>
      </div>
    </div>
    <RouterView />
    <MusicPlayer v-if="showPlayer" class="p-2" />
  </div>
</template>
