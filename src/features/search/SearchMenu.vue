<script setup lang="ts">
import router from '@/router'
import axios from 'axios'
import { computed, onMounted, ref, watch } from 'vue'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'

const search = ref('')
const currentSearch = ref('')

watch(search, (newVal) => {
  if (newVal !== currentSearch.value) {
    currentSearch.value = newVal
    searchAlbums()
  }
})

const albums = ref<Album[]>([])

const albumArtists = ref<Record<number, User[]>>({})

const searchAlbums = async () => {
  try {
    const response = await axios.get<Album[]>(`http://localhost:5091/Album/${currentSearch.value}`)
    albums.value = response.data

    for (const album of response.data) {
      await fetchAlbumArtists(album.id)
    }
  } catch (error) {
    console.error('Error fetching albums:', error)
  }
}

const handleAlbumClick = (id: number) => {
  router.push({ name: 'album', params: { id: id.toString() } })
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

const chunkedAlbums = computed(() => chunkArray(albums.value, 6))

const fetchAlbumArtists = async (albumId: number) => {
  try {
    const response = await axios.get<User[]>(`http://localhost:5091/User/album_id/${albumId}`)
    albumArtists.value[albumId] = response.data
  } catch (error) {
    console.error(`Error fetching artists for album ${albumId}:`, error)
  }
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
        v-for="(chunkedAlbum, rowIndex) in chunkedAlbums"
        :key="rowIndex"
        class="grid grid-cols-7 gap-2"
      >
        <button
          v-for="album in chunkedAlbum"
          :key="album.id"
          class="flex items-center justify-center mb-2 mt-2"
          @click="handleAlbumClick(album.id)"
        >
          <div class="flex flex-col items-center">
            <img
              :src="
                album.imagePath && album.imagePath.trim() !== ''
                  ? album.imagePath
                  : '/assets/images/album.jpeg'
              "
              class="rounded-3xl"
              width="120"
              height="120"
            />
            <div class="text-white text-lg font-arial">{{ album.name }}</div>
            <div class="text-white text-sm font-arial">
              <span v-if="albumArtists[album.id]">
                <span v-for="(artist, i) in albumArtists[album.id]" :key="artist.id">
                  {{ artist.displayName }}<span v-if="i < albumArtists[album.id].length - 1">, </span>
                </span>
              </span>
              <span v-else>Unknown Artist</span>
            </div>
          </div>
        </button>
      </div>
    </PerfectScrollbar>
  </div>
</template>
