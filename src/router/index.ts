import { createRouter, createWebHistory } from 'vue-router'
import PlaylistPage from '@/features/playlist/PlaylistPage.vue'
import AppPage from '@/features/main/AppPage.vue'
import SearchPage from '@/features/search/SearchPage.vue'
import AlbumPage from '@/features/album/AlbumPage.vue'
import LoginPage from '@/features/auth/LoginPage.vue'
import SignupPage from '@/features/auth/SignupPage.vue'
import CreatePlaylist from '@/features/playlist/CreatePlaylist.vue'
import ListeningHistory from '@/features/history/ListeningHistory.vue'
import LocalSongs from '@/features/localmusic/LocalSongs.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: AppPage
    },
    {
      path: '/playlist/:id',
      name: 'playlist',
      component: PlaylistPage,
      props: true
    },
    {
      path: '/search',
      name: 'search',
      component: SearchPage
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage
    },
    {
      path: '/signup',
      name: 'signup',
      component: SignupPage
    },
    {
      path: '/album/:id',
      name: 'album',
      component: AlbumPage,
      props: true
    },
    {
      path: '/create-playlist',
      name: 'create-playlist',
      component: CreatePlaylist
    },
    {
      path: '/listening-history/:id',
      name: 'listening-history',
      component: ListeningHistory,
      props: true
    },
    {
      path: '/local-storage',
      name: 'local-storage',
      component: LocalSongs,
      props: true
    },
  ]
})

export default router
