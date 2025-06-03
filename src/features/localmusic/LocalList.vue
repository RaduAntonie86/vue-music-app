<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { usePlayerStore } from '@/stores/usePlayerStore'
import { Song } from '@/types/Song'
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'
import { getSongFromIndexedDB, saveSongToIndexedDB } from '@/utils/localSongStorage'

const store = usePlayerStore()

const localSongs = ref<Song[]>([])
const localFilesMap = ref<Record<number, string>>({})
let localIdCounter = 1_000_000

const getAudioDuration = (file: File): Promise<number> => {
  return new Promise((resolve, reject) => {
    const url = URL.createObjectURL(file)
    const audio = new Audio(url)

    audio.addEventListener('loadedmetadata', () => {
      resolve(audio.duration)
      URL.revokeObjectURL(url) // cleanup
    })

    audio.addEventListener('error', (e) => {
      reject(e)
      URL.revokeObjectURL(url)
    })
  })
}

const addLocalSongs = async (files: FileList | null) => {
  if (!files) return

  for (const file of Array.from(files)) {
    const id = localIdCounter++
    const url = URL.createObjectURL(file)

    await saveSongToIndexedDB(id, file) // ✅ Save binary file

    let lengthInSeconds = 0
    try {
      lengthInSeconds = await getAudioDuration(file)
    } catch (error) {
      console.warn('Failed to get audio duration for', file.name, error)
    }

    localFilesMap.value[id] = url

    const newSong: Song = {
      id,
      name: file.name.replace(/\.[^/.]+$/, ''),
      length: lengthInSeconds,
      listens: 0,
      path: url,
      isLocal: true,
    }

    localSongs.value.push(newSong)
  }

  store.localBlobs = localSongs.value.map(song => localFilesMap.value[song.id] || null)
  persistLocalSongs()
}


const playSong = (song: Song) => {
  const index = localSongs.value.findIndex((s) => s.id === song.id)
  if (index === -1) return

  const songsWithBlobs = localSongs.value.map((s) => ({
    ...s,
    audioPath: localFilesMap.value[s.id] || s.path,
    isLocal: true,
  }))

  store.setPlaylist(
    songsWithBlobs,
    songsWithBlobs.map(() => []),
    songsWithBlobs.map(() => ({
      id: -1,
      name: 'Local',
      imagePath: '',
      releaseDate: Date.now().toString(),
      songIds: [],
      genreIds: [],
    })),
  )

  store.localBlobs = songsWithBlobs.map(s => s.audioPath || null)

  store.playSongByIndex(index)
}

const removeLocalSong = (index: number) => {
  const song = localSongs.value[index]
  if (!song) return

  const url = localFilesMap.value[song.id]
  if (url) {
    URL.revokeObjectURL(url)
    delete localFilesMap.value[song.id]
  }

  localSongs.value.splice(index, 1)

  store.localBlobs = localSongs.value.map(song => localFilesMap.value[song.id] || null)
  persistLocalSongs()
}

const formatLength = (seconds: number): string => {
  const mins = Math.floor(seconds / 60)
  const secs = Math.floor(seconds % 60)
  return `${mins}:${secs.toString().padStart(2, '0')}`
}

const persistLocalSongs = () => {
  localStorage.setItem('localSongs', JSON.stringify(localSongs.value.map(song => ({
    id: song.id,
    name: song.name,
    length: song.length,
  }))))
}


const loadPersistedLocalSongs = async () => {
  const savedMetadata = JSON.parse(localStorage.getItem('localSongs') || '[]')
  for (const meta of savedMetadata) {
    const file = await getSongFromIndexedDB(meta.id)
    if (!file) continue

    const url = URL.createObjectURL(file)

    localFilesMap.value[meta.id] = url

    localSongs.value.push({
      id: meta.id,
      name: meta.name,
      length: meta.length,
      listens: 0,
      path: url,
      isLocal: true,
    })
  }
}

onMounted(() => {
  loadPersistedLocalSongs()
})
</script>

<template>
  <div class="bg-[#362323] rounded-md p-4">
    <div class="flex justify-between items-center mb-4">
      <h2 class="text-white text-3xl font-semibold font-arial">Local Songs</h2>
      <label
        for="fileInput"
        class="cursor-pointer font-arial text-xl font-semibold bg-white rounded-xl px-4 py-2 text-[#882323] hover:text-[#888888]"
      >
        Add Local Songs
      </label>
      <input
        id="fileInput"
        type="file"
        accept="audio/*"
        multiple
        class="hidden"
        @change="(e) => addLocalSongs((e.target as HTMLInputElement).files)"
      />
    </div>

    <div class="grid grid-cols-8 p-2 mb-2 font-arial text-[#753E3E] font-bold">
      <div>#</div>
      <div class="col-span-3">Title</div>
      <div class="col-span-2">Length</div>
      <div class="col-span-2"></div>
    </div>

    <PerfectScrollbar class="min-h-[38vh] max-h-[38vh] overflow-hidden">
      <div
        v-for="(song, index) in localSongs"
        :key="song.id"
        class="grid grid-cols-8 p-2 font-arial font-bold items-center text-white cursor-pointer hover:text-[#888888]"
      >
        <div>{{ index + 1 }}</div>
        <div class="col-span-3" @click="playSong(song)">
          {{ song.name }}
        </div>
        <div class="col-span-2">
          {{ song.length > 0 ? formatLength(song.length) : '-' }}
        </div>
          <div class="col-span-2 flex place-items-center font-normal hover:text-xl">
            <i
              @click="removeLocalSong(index)"
              class="bi bi-dash-lg hover:text-white cursor-pointer"
            ></i>
          </div>
      </div>
      <div v-if="localSongs.length === 0" class="text-gray-400 text-center mt-10">
        No local songs loaded. Add some!
      </div>
    </PerfectScrollbar>
  </div>
</template>

