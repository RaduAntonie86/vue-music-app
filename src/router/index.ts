import { createRouter, createWebHistory } from 'vue-router'
import PlaylistPage from '@/features/playlist/PlaylistPage.vue'
import AppPage from '@/features/main/AppPage.vue'
import SearchPage from '@/features/search/SearchPage.vue'
import AlbumPage from '@/features/album/AlbumPage.vue'
import LoginPage from '@/features/auth/LoginPage.vue'
import SignupPage from '@/features/auth/SignupPage.vue'

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
    }
  ]
})

export default router
