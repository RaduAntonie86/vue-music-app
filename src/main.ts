import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import MusicPlayer from './components/MusicPlayer.vue'
import LeftSidebar from './components/LeftSidebar.vue'
import LeftSideButtons from './components/LeftSideButtons.vue'
import MainPage from './features/main/MainPage.vue'
import SearchPage from './features/search/SearchPage.vue'
import AlbumPage from './features/auth/album/AlbumList.vue'
import AlbumPreview from './features/auth/album/AlbumPreview.vue'
import TrackBar from './components/common/TrackBar.vue'
import PlaylistPreview from './features/playlist/PlaylistPreview.vue'
import IconButton from './components/common/IconButton.vue'
import Albums from './features/auth/album/AlbumList.vue'

import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"
import "bootstrap-icons/font/bootstrap-icons.css"

import { PerfectScrollbarPlugin } from 'vue3-perfect-scrollbar';
import 'vue3-perfect-scrollbar/style.css';

import App from './App.vue'
import router from './router'
import SearchMenu from './features/search/SearchMenu.vue'
import PlaylistList from './features/playlist/PlaylistList.vue'

const app = createApp(App)
app.use(createPinia())

app.component('music-player', MusicPlayer)
app.component('left-sidebar', LeftSidebar)
app.component('playlist-list', PlaylistList)
app.component('left-side-buttons', LeftSideButtons)
app.component('main-page', MainPage)
app.component('search-page', SearchPage)
app.component('search-menu', SearchMenu)
app.component('album-page', AlbumPage)
app.component('album-list', Albums)
app.component('album-preview', AlbumPreview)
app.component('track-bar', TrackBar)
app.component('playlist-preview', PlaylistPreview)
app.component('icon-button', IconButton)

createApp(App).use(PerfectScrollbarPlugin).mount('#app')
createApp(App).use(router).mount('#app')

app.use(createPinia())
app.use(router)

app.mount('#app')
