<script setup lang="ts">
import router from '@/router'
import { Album } from '@/types/Album'
import { Playlist } from '@/types/Playlist'
import { User } from '@/types/User'
import { onMounted, ref, watch } from 'vue'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const search = ref('')
const currentSearch = ref('')

onMounted(() => {
  searchAlbums()
})

watch(search, (newVal) => {
  if (newVal !== currentSearch.value) {
    currentSearch.value = newVal
    searchAlbums()
  }
})

const albums = ref<Album[]>([])

const playlists = ref<Playlist[]>([])

const albumArtists = ref<Record<number, User[]>>({})

const combinedMedia = ref<(Album | Playlist)[]>([])

const searchAlbums = async () => {
  try {
    let albumRes: Album[] = []
    let playlistRes: Playlist[] = []

    if (!currentSearch.value || currentSearch.value.trim() === '') {
      albumRes = await Album.fetchAll()
      playlistRes = await Playlist.fetchAll()
    } else {
      albumRes = await Album.fetchAlbumsByName(currentSearch.value)
      playlistRes = await Playlist.fetchPlaylistsByName(currentSearch.value)
    }

    albums.value = albumRes
    console.log('Albums:', albumRes)
    playlists.value = playlistRes

    for (const album of albumRes) {
      await fetchAlbumArtists(album.id!)
    }

    combinedMedia.value = [...albumRes, ...playlistRes]
  } catch (error) {
    console.error('Error fetching albums or playlists:', error)
  }
}

const handleItemClick = (item: Album | Playlist) => {
  if ('releaseDate' in item) {
    router.push({ name: 'album', params: { id: item.id!.toString() } })
  } else {
    router.push({ name: 'playlist', params: { id: item.id!.toString() } })
  }
}

const chunkArray = (arr: any[], size: number) => {
  const chunks = []
  for (let i = 0; i < arr.length; i += size) {
    chunks.push(arr.slice(i, i + size))
  }
  return chunks
}

onMounted(() => {
  searchAlbums()
})

const fetchAlbumArtists = async (albumId: number) => {
  try {
    const artists = await User.fetchAlbumArtists(albumId)
    albumArtists.value = { ...albumArtists.value, [albumId]: artists }
    console.log(`Artists for album ${albumId}:`, artists)
  } catch (error) {
    console.error(`Error fetching artists for album ${albumId}:`, error)
  }
}

const isAlbum = (item: Album | Playlist): item is Album => {
  return 'releaseDate' in item && item.releaseDate !== undefined && item.releaseDate !== null
}
</script>

<template>
  <div class="bg-[#362323] rounded-md overflow-hidden">
    <div class="d-flex w-[50vh] h-[7vh] p-3 mb-3" role="search">
      <input
        v-model="search"
        class="form-control me-2 input-rounded text-[#efd0d0] bg-custom"
        type="search"
        placeholder="Search"
        aria-label="Search"
      />
    </div>
    <PerfectScrollbar class="min-h-[75vh] max-h-[75vh] overflow-hidden">
      <div
        v-for="(chunkedItem, rowIndex) in chunkArray(combinedMedia, 6)"
        :key="rowIndex"
        class="grid grid-cols-7 gap-2"
      >
        <button
          v-for="item in chunkedItem"
          :key="item.id"
          class="flex items-center justify-center mb-2 mt-2"
          @click="handleItemClick(item)"
        >
          <div class="flex flex-col items-center">
            <img
              :src="
                item.imagePath && item.imagePath.trim() !== ''
                  ? item.imagePath
                  : 'images/albums/album.jpeg'
              "
              class="rounded-3xl"
              width="120"
              height="120"
            />
            <div class="text-white text-lg font-arial">{{ item.name }}</div>
            <div class="text-white text-sm font-arial">
              <template v-if="isAlbum(item)">
                <span v-if="item.id && albumArtists[item.id]">
                  <span v-for="(artist, i) in albumArtists[item.id]" :key="artist.id">
                    {{ artist.displayName
                    }}<span v-if="i < albumArtists[item.id].length - 1">, </span>
                  </span>
                </span>
                <span v-else>Unknown Artist</span>
              </template>
              <template v-else>
                {{ item.description }}
              </template>
            </div>
          </div>
        </button>
      </div>
    </PerfectScrollbar>
  </div>
</template>
