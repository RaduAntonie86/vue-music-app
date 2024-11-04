<script setup lang="ts">
import { onMounted, ref, reactive } from 'vue'
import { useMediaControls } from '@vueuse/core'
import song from '@/assets/song.mp3';

// Define the song object
const currentSong = reactive({
  title: 'Song Name',
  artist: 'Artist Name',
  src: song, // You can replace this with dynamic data if needed
})

// Create a ref for the audio element
const audio = ref<HTMLAudioElement | null>(null)

// Use media controls with the song's src property
const { playing, currentTime, duration, volume } = useMediaControls(audio, {
  src: currentSong.src,  // Set the source from the song object
})

// Change initial media properties when the component is mounted
onMounted(() => {
  volume.value = 0.5
  currentTime.value = 0
})

function previousSong(){
  currentTime.value = 0
}

</script>

<template>
<audio ref="audio"></audio>
<div class="grid-cols-3 grid bg-[#362323]">
    <div class="flex align-middle place-items-center">
        <img class="rounded-3xl p-1" src="../assets/images/album.jpeg" width="70" height="70" style="margin-right: 10px;">
        <div>
          <div class="text-white text-lg font-arial">{{ currentSong.title }}</div> <!-- Dynamic song title -->
          <div class="text-white text-xs font-arial">{{ currentSong.artist }}</div> <!-- Dynamic artist name -->
        </div>
    </div>
    <div class="justify-center items-center flex">
        <i class="bi bi-shuffle mr-16 text-xl text-white"></i>
        <button @click="previousSong">
            <i class="bi bi-skip-backward-fill mr-5 text-[22px] text-white"></i>
        </button>
        <button v-if="!playing" @click="playing = !playing">
            <i class="bi bi-play-fill text-[25px]" style="color: white;"></i>
        </button>
        <button v-if="playing" @click="playing = !playing">
            <i class="bi bi-pause-fill text-[25px]" style="color: white;"></i>
        </button>
        <i class="bi bi-skip-forward-fill ml-5 text-[22px] text-white"></i>
        <i class="bi bi-repeat ml-16 text-xl text-white"></i>
    </div>
    <div class="grid justify-items-end place-items-center p-2">
      <track-bar width="220" height="11" percent="55"></track-bar>
    </div>
</div>
</template>
