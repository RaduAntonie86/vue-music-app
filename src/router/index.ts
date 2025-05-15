import { createRouter, createWebHistory } from 'vue-router'
import PlaylistPage from '@/components/PlaylistPage.vue'
import AppPage from '@/components/AppPage.vue'
import SearchPage from '@/components/SearchPage.vue'
import AlbumPage from '@/components/AlbumPage.vue'
import LoginPage from '@/components/LoginPage.vue'
import SignupPage from '@/components/SignupPage.vue'

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
      path: '/album',
      name: 'album',
      component: AlbumPage
    }
  ]
})

export default router
