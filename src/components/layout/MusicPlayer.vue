<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useMediaControls } from '@vueuse/core'
import { usePlayerStore } from '@/stores/usePlayerStore'

const store = usePlayerStore()

const audio = ref<HTMLAudioElement | null>(null)

const songPath = computed(() =>
  store.currentSong ? `http://localhost:5091/Song/stream/${store.currentSong.id}` : ''
)

const { playing, volume } = useMediaControls(audio)

const currentSong = computed(() => store.currentSong)

watch(
  () => store.playlist,
  (val) => {
    console.log(
      'Playlist changed',
      val.map((s) => s.name)
    )
  }
)

watch(currentSong, (newSong) => {
  if (newSong && audio.value) {
    const tryPlay = () => {
      audio.value
        ?.play()
        .then(() => {
          playing.value = true
        })
        .catch((err) => {
          console.error('Autoplay failed:', err)
        })

      audio.value?.removeEventListener('canplay', tryPlay)
    }

    audio.value.addEventListener('canplay', tryPlay, { once: true })

    audio.value.load()
  }
})

const currentTime = ref(0)
const duration = ref(0)

watch(audio, (el) => {
  if (el) {
    el.addEventListener('timeupdate', () => {
      currentTime.value = el.currentTime
    })
    el.addEventListener('loadedmetadata', () => {
      duration.value = el.duration
    })
    el.addEventListener('ended', nextSong)
  }
})

function seekAudio(percent: number) {
  if (audio.value && duration.value) {
    const newTime = (percent / 100) * duration.value
    audio.value.currentTime = newTime
    currentTime.value = newTime
  }
}

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

function pauseSong() {
  if (audio.value) {
    playing.value = false
    audio.value.pause()
  }
}

function togglePlayPause() {
  if (playing.value) {
    pauseSong()
  } else {
    playSong()
  }
}

function handleAudioError() {
  console.error('Error loading audio')
}

const nextSong = () => {
  if (store.currentIndex < store.playlist.length - 1) {
    store.playSongByIndex(store.currentIndex + 1)
  } else if (repeat.value) store.playSongByIndex(0)
}

const selectNextSong = () => {
  if (store.currentIndex < store.playlist.length - 1) {
    store.playSongByIndex(store.currentIndex + 1)
  } else store.playSongByIndex(0)
}

const selectPreviousSong = () => {
  if (audio.value) {
    if (audio.value.currentTime > 1) {
      audio.value.currentTime = 0
    } else if (store.currentIndex > 0) {
      store.playSongByIndex(store.currentIndex - 1)
    } else {
      store.playSongByIndex(store.playlist.length - 1)
    }
  }
}
const repeat = ref(false)

const shuffle = ref(false)

const muted = ref(false)
const previousVolume = ref(volume.value)

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
}

const imageSource = computed(() => {
  const path = store.currentAlbum?.imagePath?.trim();
  return path ? path : 'images/albums/album.jpeg';
});
</script>

<template>
  <div class="bg-[#362323]">
    <div class="grid grid-cols-3 md:grid-cols-5 gap-2 items-center">
      <div class="flex align-middle items-center">
        <img
          class="rounded-3xl mr-[10px]"
          :src="imageSource"
          width="70"
          height="70"
        />
        <div>
          <div class="text-white text-lg font-arial">
            {{ store.currentSong?.name || 'No song playing' }}
          </div>
          <div class="text-white text-xs font-arial">
            {{
              store.currentArtists.map((artist) => artist.displayName).join(', ') ||
              'Unknown Artist'
            }}
          </div>
        </div>
      </div>

      <icon-button
        iconName="bi-shuffle"
        @click="toggleShuffle"
        :class="[
          'hidden md:flex justify-center items-center h-full text-xl hover:text-2xl',
          shuffle ? 'text-[#888888]' : 'text-white'
        ]"
      ></icon-button>

      <div class="flex flex-col items-center justify-center h-full">
        <div class="flex items-center">
          <icon-button
            @click="selectPreviousSong"
            iconName="bi-skip-backward-fill"
            class="mr-5 text-[22px] text-white hover:text-2xl"
          ></icon-button>
          <icon-button
            @click="togglePlayPause"
            :iconName="playing ? 'bi-pause-fill' : 'bi-play-fill'"
            class="text-[25px] text-white hover:text-2xl"
          ></icon-button>
          <icon-button
            @click="selectNextSong"
            iconName="bi-skip-forward-fill"
            class="ml-5 text-[22px] text-white hover:text-2xl"
          ></icon-button>
        </div>

        <div class="flex justify-center w-full mt-2">
          <track-bar
            width="250"
            height="9"
            :percent="(currentTime / duration) * 100"
            @input="seekAudio"
          />
        </div>
      </div>

      <icon-button
        iconName="bi-repeat"
        @click="toggleRepeat"
        :class="[
          'hidden md:flex justify-center items-center h-full text-xl hover:text-2xl',
          repeat ? 'text-[#888888]' : 'text-white'
        ]"
      ></icon-button>

      <div class="flex items-center">
        <icon-button
          :iconName="muted ? 'bi-volume-mute-fill' : 'bi-volume-up-fill'"
          @click="toggleMute"
          class="mr-2 text-white text-2xl hover:text-3xl"
        ></icon-button>
        <track-bar width="220" height="11" :percent="volume * 100" @input="setVolume" />
      </div>
    </div>
  </div>

  <audio ref="audio" :src="songPath" @error="handleAudioError"></audio>
</template>
