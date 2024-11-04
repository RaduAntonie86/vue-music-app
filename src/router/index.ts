import { createRouter, createWebHistory } from 'vue-router'
import PlaylistPage from '@/components/PlaylistPage.vue'
import MainPage from '@/components/MainPage.vue'
import SearchPage from '@/components/SearchPage.vue'
import AlbumPage from '@/components/AlbumPage.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: MainPage
    },
    {
      path: '/playlist',
      name: 'playlist',
      component: PlaylistPage
    },
    {
      path: '/search',
      name: 'search',
      component: SearchPage
    },
    {
      path: '/album',
      name: 'album',
      component: AlbumPage
    }
  ]
})

export default router
