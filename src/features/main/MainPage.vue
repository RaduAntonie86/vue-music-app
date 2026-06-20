<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'
import { computed, onMounted, ref, watch } from 'vue'
import { Album } from '@/types/Album'
import { Playlist } from '@/types/Playlist'
import { useAuthStore } from '@/stores/authStore'
import { ListeningHistory } from '@/types/ListeningHistory'
import { User } from '@/types/User'
import router from '@/router'

const authStore = useAuthStore()

const popularAlbums = ref<Album[]>([])
const userPlaylists = ref<Playlist[]>([])
const recentAlbums = ref<Album[]>([])
const albumArtistNames = ref<Record<number, string>>({})
const playlistAuthorNames = ref<Record<number, string>>({})

const isLoggedIn = computed(() => authStore.isLoggedIn)

const fetchAlbumArtistNames = async (albums: Album[]) => {
  await Promise.all(
    albums.map(async (album) => {
      if (!album.id) return

      try {
        const artists = await User.fetchAlbumArtists(album.id)
        albumArtistNames.value[album.id] = artists?.length
          ? artists.map((artist) => artist.displayName || artist.username).join(', ')
          : 'Unknown Artist'
      } catch (error) {
        console.error(`Error fetching artists for album ${album.id}:`, error)
        albumArtistNames.value[album.id] = 'Unknown Artist'
      }
    })
  )
}

const fetchPlaylistAuthorNames = async (playlists: Playlist[]) => {
  await Promise.all(
    playlists.map(async (playlist) => {
      if (!playlist.id) return

      try {
        const users = playlist.userIds?.length
          ? await User.fetchUsersByIds(playlist.userIds)
          : []
        playlistAuthorNames.value[playlist.id] = users?.length
          ? users.map((user) => user.displayName || user.username).join(', ')
          : 'Unknown Author'
      } catch (error) {
        console.error(`Error fetching authors for playlist ${playlist.id}:`, error)
        playlistAuthorNames.value[playlist.id] = 'Unknown Author'
      }
    })
  )
}

const fetchPopularAlbums = async () => {
  try {
    popularAlbums.value = await Album.fetchTop10()
    await fetchAlbumArtistNames(popularAlbums.value)
  } catch (error) {
    console.error('Error fetching popular albums:', error)
  }
}

const fetchUserPlaylists = async () => {
  if (!authStore.userId) return

  try {
    userPlaylists.value = await Playlist.fetchFromUser(authStore.userId)
    await fetchPlaylistAuthorNames(userPlaylists.value)
  } catch (error) {
    console.error('Error fetching user playlists:', error)
  }
}

const fetchRecentAlbums = async () => {
  if (!authStore.userId) return

  try {
    const history = await ListeningHistory.fetchByUserId(authStore.userId)
    const uniqueSongIds = [...new Set(history.map((entry) => entry.songId))]

    const albumPromises = uniqueSongIds.map((songId) => Album.fetchAlbumForSong(songId))
    const albums = await Promise.all(albumPromises)

    const seen = new Set<number>()
    recentAlbums.value = albums.filter((album) => {
      if (!album?.id || seen.has(album.id)) return false
      seen.add(album.id)
      return true
    })

    await fetchAlbumArtistNames(recentAlbums.value)
  } catch (error) {
    console.error('Error fetching recent albums:', error)
  }
}

const loadHomeData = async () => {
  await fetchPopularAlbums()
  if (authStore.isLoggedIn) {
    await Promise.all([fetchUserPlaylists(), fetchRecentAlbums()])
  } else {
    userPlaylists.value = []
    recentAlbums.value = []
  }
}

watch(
  () => authStore.isLoggedIn,
  () => {
    loadHomeData()
  }
)

onMounted(() => {
  loadHomeData()
})

const goToAlbum = (albumId: number) => {
  router.push({ name: 'album', params: { id: albumId.toString() } })
}

const goToPlaylist = (playlistId: number) => {
  router.push({ name: 'playlist', params: { id: playlistId.toString() } })
}
</script>

<template>
  <PerfectScrollbar class="bg-[#362323] rounded-md overflow-hidden h-[87vh]">
    <div class="font-arial text-white ml-6 mt-4 mr-6 pb-6">
      <div class="mb-8">
        <p class="text-3xl mb-4">Most Popular Albums</p>
        <div class="grid grid-cols-5 gap-3">
          <button
            v-for="album in popularAlbums"
            :key="album.id"
            class="text-left"
            @click="goToAlbum(album.id!)"
          >
            <album-preview
              :albumName="album.name"
              :artistName="albumArtistNames[album.id!] || 'Unknown Artist'"
              :imagePath="album.imagePath || 'images/albums/album.jpeg'"
            />
          </button>
        </div>
      </div>

      <div v-if="isLoggedIn" class="mb-8">
        <p class="text-3xl mb-4">Your Playlists</p>
        <div class="grid grid-cols-5 gap-3">
          <button
            v-for="playlist in userPlaylists"
            :key="playlist.id"
            class="text-left"
            @click="goToPlaylist(playlist.id!)"
          >
            <div class="flex flex-col items-center">
              <img
                class="rounded-3xl min-h-[100px] min-w-[100px]"
                :src="playlist.imagePath || '/images/albums/album.jpeg'"
                width="120"
                height="120"
                style="margin-bottom: 5px"
              />
              <div class="text-white text-lg font-arial text-center">{{ playlist.name }}</div>
              <div class="text-white text-sm font-arial text-center">
                {{ playlistAuthorNames[playlist.id!] || 'Unknown Author' }}
              </div>
            </div>
          </button>
        </div>
      </div>

      <div v-if="isLoggedIn" class="mb-8">
        <p class="text-3xl mb-4">Recently Listened</p>
        <div class="grid grid-cols-5 gap-3">
          <button
            v-for="album in recentAlbums"
            :key="album.id"
            class="text-left"
            @click="goToAlbum(album.id!)"
          >
            <album-preview
              :albumName="album.name"
              :artistName="albumArtistNames[album.id!] || 'Unknown Artist'"
              :imagePath="album.imagePath || 'images/albums/album.jpeg'"
            />
          </button>
        </div>
      </div>
    </div>
  </PerfectScrollbar>
</template>
