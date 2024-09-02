import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import MusicPlayer from './components/MusicPlayer.vue'
import MainMenu from './components/MainMenu.vue'
import LeftSidebar from './components/LeftSidebar.vue'
import PlaylistPage from './components/PlaylistPage.vue'
import LeftSideButtons from './components/LeftSideButtons.vue'
import MainPage from './components/MainPage.vue'
import SearchPage from './components/SearchPage.vue'
import AlbumPage from './components/AlbumPage.vue'

import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
import "bootstrap-icons/font/bootstrap-icons.css"

import { PerfectScrollbarPlugin } from 'vue3-perfect-scrollbar';
import 'vue3-perfect-scrollbar/style.css';

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.component('music-player', MusicPlayer)
app.component('main-menu', MainMenu)
app.component('left-sidebar', LeftSidebar)
app.component('playlist-page', PlaylistPage)
app.component('left-side-buttons', LeftSideButtons)
app.component('main-page', MainPage)
app.component('search-page', SearchPage)
app.component('album-page', AlbumPage)

createApp(App).use(PerfectScrollbarPlugin).mount('#app')

app.use(createPinia())
app.use(router)

app.mount('#app')
