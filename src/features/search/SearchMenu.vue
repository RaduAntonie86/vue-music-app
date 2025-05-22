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

const searchAlbums = async () => {
  try {
    const response = await axios.get<Album[]>(
      `http://localhost:5091/Album/${currentSearch.value}`
    )
    albums.value = response.data
  } catch (error) {
    console.error('Error fetching songs from playlist:', error)
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
              class="rounded-3xl"
              src="../../assets/images/album.jpeg"
              width="120"
              height="120"
              style="margin-bottom: 5px"
            />
            <div class="text-white text-lg font-arial">{{ album.name }}</div>
            <div class="text-white text-sm font-arial">Artist Name</div>
          </div>
        </button>
      </div>
    </PerfectScrollbar>
  </div>
</template>
