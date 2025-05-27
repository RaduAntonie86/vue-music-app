<script setup lang="ts">
import router from '@/router'
import axios from 'axios'
import { computed, onMounted, ref, watch } from 'vue'
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
    let albumRes, playlistRes

    if (!currentSearch.value || currentSearch.value.trim() === '') {
      albumRes = await axios.get<Album[]>('http://localhost:5091/Album')
      playlistRes = await axios.get<Playlist[]>('http://localhost:5091/Playlist')
    } else {
      albumRes = await axios.get<Album[]>(`http://localhost:5091/Album/name/${currentSearch.value}`)
      playlistRes = await axios.get<Playlist[]>(
        `http://localhost:5091/Playlist/name/${currentSearch.value}`
      )
    }

    albums.value = albumRes.data
    console.log('Albums:', albumRes.data)
    playlists.value = playlistRes.data

    for (const album of albumRes.data) {
      await fetchAlbumArtists(album.id!)
    }

    combinedMedia.value = [...albumRes.data, ...playlistRes.data]
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
    const response = await axios.get<User[]>(`http://localhost:5091/User/album_id/${albumId}`)
    // replace entire albumArtists object with updated property to trigger reactivity
    albumArtists.value = { ...albumArtists.value, [albumId]: response.data }
    console.log(`Artists for album ${albumId}:`, response.data)
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
                    {{ artist.displayName }}<span v-if="i < albumArtists[item.id].length - 1">, </span>
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
