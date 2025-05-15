<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useMediaControls } from '@vueuse/core'
import { usePlayerStore } from '@/usePlayerStore'

const store = usePlayerStore()

// Create a ref for the audio element
const audio = ref<HTMLAudioElement | null>(null)

const songPath = computed(() =>
  store.currentSong ? `http://localhost:5091/Song/stream/${store.currentSong.id}` : ''
)

// Use media controls with the song's src property
const { playing, volume } = useMediaControls(audio)

const currentSong = computed(() => store.currentSong)

watch(currentSong, (newSong) => {
  if (newSong && audio.value) {
    // Wait until the audio is ready before trying to play
    const tryPlay = () => {
      audio.value?.play()
        .then(() => {
          playing.value = true
        })
        .catch(err => {
          console.error('Autoplay failed:', err)
        })

      // Remove listener after firing
      audio.value?.removeEventListener('canplay', tryPlay)
    }

    // Attach listener once, then try to play
    audio.value.addEventListener('canplay', tryPlay, { once: true })
    
    // Force reload the audio when the src changes
    audio.value.load()
  }
})


function playSong() {
  if (audio.value) {
    audio.value
      .play()
      .then(() => {
        playing.value = true
      })
      .catch((err) => {
        console.error('Autoplay failed:', err)
      })
  }
}

// Function to pause the song
function pauseSong() {
  if (audio.value) {
    playing.value = false
    audio.value.pause()
    //console.log("Audio is paused");
  }
}

function togglePlayPause() {
  if (playing.value) {
    pauseSong()
  } else {
    playSong()
  }
}

// Function to handle audio errors
function handleAudioError() {
  console.error('Error loading audio')
}

const nextSong = () => {
  if (store.currentIndex < store.playlist.length - 1) {
    store.playSongByIndex(store.currentIndex + 1)
  }
  else
    store.playSongByIndex(0)
}

const previousSong = () => {
  if (audio.value) {
    if (audio.value.currentTime > 1) {
      audio.value.currentTime = 0
    } 
    else if (store.currentIndex > 0) {
      store.playSongByIndex(store.currentIndex - 1)
    }
    else {
      store.playSongByIndex(store.playlist.length - 1)
    }
  }
}
</script>

<template>
  <div class="bg-[#362323]">
    <div class="grid grid-cols-3 md:grid-cols-5 gap-2 text-white items-center">
      <div class="flex align-middle items-center">
        <img
          class="rounded-3xl mr-[10px]"
          src="../assets/images/album.jpeg"
          width="70"
          height="70"
        />
        <div>
          <div class="text-white text-lg font-arial">
            {{ store.currentSong?.name || 'No song playing' }}
          </div>
          <div class="text-white text-xs font-arial">
            {{ 'Unknown Artist' }}
          </div>
        </div>
      </div>

      <button class="hidden md:flex justify-center items-center h-full">
        <i class="bi bi-shuffle text-xl text-white"></i>
      </button>

      <div class="flex flex-col items-center justify-center h-full">
        <div class="flex items-center">
          <icon-button
            @click="previousSong"
            iconName="bi-skip-backward-fill"
            class="mr-5 text-[22px]"
          ></icon-button>
          <icon-button
            @click="togglePlayPause"
            :iconName="playing ? 'bi-pause-fill' : 'bi-play-fill'"
            class="text-[25px]"
          ></icon-button>
          <icon-button
            @click="nextSong"
            iconName="bi-skip-forward-fill"
            class="ml-5 text-[22px]"
          ></icon-button>
        </div>

        <div class="flex justify-center w-full mt-2">
          <track-bar width="250" height="9" percent="55"></track-bar>
        </div>
      </div>

      <icon-button
        iconName="bi-repeat"
        class="hidden md:flex justify-center items-center h-full text-xl"
      ></icon-button>

      <div class="flex items-center">
        <icon-button iconName="bi-volume-up-fill mr-2 text-2xl"></icon-button>
        <track-bar
          width="220"
          height="11"
          :percent="volume * 100"
          @input="
            (newVolume: number) => {
              volume = newVolume / 100
            }
          "
        >
        </track-bar>
      </div>
    </div>
  </div>

  <audio ref="audio" :src="songPath" @error="handleAudioError"></audio>
</template>
