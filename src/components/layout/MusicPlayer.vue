<script setup lang="ts">
import { ref, computed, watch, onMounted, onBeforeUnmount } from 'vue'
import { usePlayerStore } from '@/stores/usePlayerStore'
import type { Song } from '@/types/Song'
import type { User } from '@/types/User'
import axios from 'axios'
import { useAuthStore } from '@/stores/authStore'
import router from '@/router'

const secondsListened = ref(0)
const store = usePlayerStore()
const audio = ref<HTMLAudioElement | null>(null)
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL
const authStore = useAuthStore()

const playing = ref(false)
const volume = ref(0.5)
const currentTime = ref(0)
const duration = ref(0)
const repeat = ref(false)
const shuffle = ref(false)
const muted = ref(false)
const previousVolume = ref(volume.value)
const MINIMUM_LISTENING_TIME = 10
let lastTime = 0

const songPath = computed(() => {
  if (store.localBlobs[store.currentIndex]) {
    return store.localBlobs[store.currentIndex]!
  }
  if (store.currentSong) {
    return `${API_BASE_URL}/Song/stream/${store.currentSong.id}`
  }
  return ''
})

const currentSong = computed(() => store.currentSong)

const imageSource = computed(() => {
  const path = store.currentAlbum?.imagePath?.trim()
  if (!path) return '/images/albums/album.jpeg'
  if (path.startsWith('/') || path.includes('images/')) return path
  return `/images/albums/${path}`
})

watch(
  () => store.playlist,
  (val: Song[]) => {
    console.log('Playlist changed:', val.map(s => s.name))
  }
)

watch(currentSong, async (newSong, oldSong) => {
  if (oldSong && secondsListened.value >= MINIMUM_LISTENING_TIME) {
    await sendListeningHistory(oldSong.id, Math.floor(secondsListened.value))
    console.log('Sent listening history for:', oldSong.name)
  }
  secondsListened.value = 0
  lastTime = 0
  currentTime.value = 0

  if (newSong && audio.value) {
    audio.value.load()
    
    await new Promise<void>((resolve, reject) => {
      const onLoaded = () => {
        audio.value?.removeEventListener('loadedmetadata', onLoaded)
        resolve()
      }
      audio.value?.addEventListener('loadedmetadata', onLoaded)

      setTimeout(() => reject(new Error('Timeout loading metadata')), 5000)
    })

    try {
      await audio.value.play()
      playing.value = true
    } catch (err) {
      console.error('Autoplay failed:', err)
    }
  }
})

function setupAudioListeners() {
  if (!audio.value) return

  audio.value.addEventListener('timeupdate', onTimeUpdate)
  audio.value.addEventListener('loadedmetadata', onLoadedMetadata)
  audio.value.addEventListener('ended', onEnded)
  audio.value.addEventListener('error', onAudioError)
}

function removeAudioListeners() {
  if (!audio.value) return

  audio.value.removeEventListener('timeupdate', onTimeUpdate)
  audio.value.removeEventListener('loadedmetadata', onLoadedMetadata)
  audio.value.removeEventListener('ended', onEnded)
  audio.value.removeEventListener('error', onAudioError)
}

function onTimeUpdate() {
  if (!audio.value) return
  const current = audio.value.currentTime
  currentTime.value = current
  
  const delta = current - lastTime
  
  if (delta > 0 && delta < 1) {
    secondsListened.value += delta
  }
  
  lastTime = current
}

function onLoadedMetadata() {
  if (!audio.value) return
  duration.value = audio.value.duration
  lastTime = 0
  secondsListened.value = 0
}

function onEnded() {
  nextSong()
}

function onAudioError() {
  console.error('Error loading audio')
}

onMounted(() => {
  setupAudioListeners()
})

onBeforeUnmount(() => {
  removeAudioListeners()
})

function playSong() {
  if (!audio.value) return
  audio.value.play().then(() => {
    playing.value = true
  }).catch(err => {
    console.error('Play failed:', err)
  })
}

function pauseSong() {
  if (!audio.value) return
  audio.value.pause()
  playing.value = false
}

function togglePlayPause() {
  if (playing.value) pauseSong()
  else playSong()
}

function seekAudio(percent: number) {
  if (!audio.value || !duration.value) return
  const newTime = (percent / 100) * duration.value
  audio.value.currentTime = newTime
  currentTime.value = newTime
}

function nextSong() {
  if (store.currentIndex < store.playlist.length - 1) {
    store.playSongByIndex(store.currentIndex + 1)
  } else if (repeat.value) {
    store.playSongByIndex(0)
  } else {
    pauseSong()
  }
}

function selectNextSong() {
  if (store.currentIndex < store.playlist.length - 1) {
    store.playSongByIndex(store.currentIndex + 1)
  } else {
    store.playSongByIndex(0)
  }
}

function selectPreviousSong() {
  if (!audio.value) return
  if (audio.value.currentTime > 1) {
    audio.value.currentTime = 0
  } else if (store.currentIndex > 0) {
    store.playSongByIndex(store.currentIndex - 1)
  } else {
    store.playSongByIndex(store.playlist.length - 1)
  }
}

function toggleRepeat() {
  repeat.value = !repeat.value
}

function toggleShuffle() {
  shuffle.value = !shuffle.value
  if (shuffle.value) {
    store.shufflePlaylist()
  } else {
    store.restoreOriginalOrder()
  }
}

function toggleMute() {
  muted.value = !muted.value
  if (muted.value) {
    previousVolume.value = volume.value
    volume.value = 0
  } else {
    volume.value = previousVolume.value || 0.5
  }
}

function setVolume(newPercent: number) {
  volume.value = newPercent / 100
  if (volume.value > 0) muted.value = false
  if (audio.value) audio.value.volume = volume.value
}

async function sendListeningHistory(songId: number, listeningTime: number) {
  if (!authStore.isLoggedIn) {
    console.log('No user logged in. Skipping listening history.')
    return
  }
  try {
    await axios.post(`${API_BASE_URL}/ListeningHistory/listen`, {
      userId: authStore.userId,
      songId,
      listeningTime
    })
  } catch (error) {
    console.error('Failed to send listening history:', error)
  }
}

function handleHistoryClick() {
  router.push({ name: 'listening-history', params: { id: authStore.userId } })
}
</script>

<template>
  <div class="bg-[#362323] p-3">
    <div class="grid grid-cols-3 md:grid-cols-5 gap-2 items-center">
      <div class="flex items-center">
        <img
          class="rounded-3xl mr-3"
          :src="imageSource"
          width="70"
          height="70"
          alt="Album Cover"
          loading="lazy"
        />
        <div>
          <div class="text-white text-lg font-arial font-semibold">
            {{ store.currentSong?.name || 'No song playing' }}
          </div>
          <div class="text-white text-xs font-arial">
            {{
              (store.currentArtists as User[])
                .map(artist => artist.displayName)
                .join(', ') || 'Unknown Artist'
            }}
          </div>
        </div>
      </div>

      <icon-button
        iconName="bi-shuffle"
        @click="toggleShuffle"
        :class="[
          'hidden md:flex justify-center items-center h-full text-xl hover:text-2xl cursor-pointer',
          shuffle ? 'text-[#888888]' : 'text-white'
        ]"
        aria-label="Toggle Shuffle"
      ></icon-button>

      <div class="flex flex-col items-center justify-center h-full">
        <div class="flex items-center">
          <icon-button
            @click="selectPreviousSong"
            iconName="bi-skip-backward-fill"
            class="mr-5 text-[22px] text-white hover:text-2xl cursor-pointer"
            aria-label="Previous Song"
          ></icon-button>
          <icon-button
            @click="togglePlayPause"
            :iconName="playing ? 'bi-pause-fill' : 'bi-play-fill'"
            class="text-[25px] text-white hover:text-2xl cursor-pointer"
            aria-label="Play/Pause"
          ></icon-button>
          <icon-button
            @click="selectNextSong"
            iconName="bi-skip-forward-fill"
            class="ml-5 text-[22px] text-white hover:text-2xl cursor-pointer"
            aria-label="Next Song"
          ></icon-button>
        </div>

        <div class="flex justify-center w-full mt-2">
          <track-bar
            :width="250"
            :height="9"
            :percent="duration ? (currentTime / duration) * 100 : 0"
            @input="seekAudio"
          />
        </div>
      </div>

      <icon-button
        iconName="bi-repeat"
        @click="toggleRepeat"
        :class="[
          'hidden md:flex justify-center items-center h-full text-xl hover:text-2xl cursor-pointer',
          repeat ? 'text-[#888888]' : 'text-white'
        ]"
        aria-label="Toggle Repeat"
      ></icon-button>

      <div class="flex items-center">
        <icon-button
          iconName="bi-clock-history"
          @click="handleHistoryClick"
          class="mr-2 text-white text-xl hover:text-2xl cursor-pointer"
          aria-label="Listening History"
        ></icon-button>
        <icon-button
          :iconName="muted ? 'bi-volume-mute-fill' : 'bi-volume-up-fill'"
          @click="toggleMute"
          class="mr-2 text-white text-2xl hover:text-3xl cursor-pointer"
          aria-label="Toggle Mute"
        ></icon-button>
        <track-bar :width="220" :height="11" :percent="volume * 100" @input="setVolume" />
      </div>
    </div>

    <audio ref="audio" :src="songPath" preload="metadata" crossorigin="anonymous"></audio>
  </div>
</template>
