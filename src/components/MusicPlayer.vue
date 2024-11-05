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
  <div class="bg-[#362323]">
    <div class="grid grid-cols-3 md:grid-cols-5 gap-2 text-white items-center"> <!-- Centers items vertically -->

      <!-- Song info and image -->
      <div class="flex align-middle items-center">
        <img class="rounded-3xl mr-[10px]" src="../assets/images/album.jpeg" width="70" height="70">
        <div> 
          <div class="text-white text-lg font-arial">{{ currentSong.title }}</div> <!-- Dynamic song title -->
          <div class="text-white text-xs font-arial">{{ currentSong.artist }}</div> <!-- Dynamic artist name -->
        </div> 
      </div>

      <!-- Shuffle button, visible on medium screens and centered vertically -->
      <button class="hidden md:flex justify-center items-center h-full">
        <i class="bi bi-shuffle text-xl text-white"></i>
      </button>

      <!-- Media controls section with centered track bar -->
      <div class="flex flex-col items-center justify-center h-full">
        <div class="flex items-center">
          <icon-button @click="previousSong"
          iconName="bi-skip-backward-fill" class="mr-5 text-[22px]"></icon-button>
          <icon-button v-if="!playing" @click="playing = !playing"
          iconName="bi-play-fill" class="text-[25px]"></icon-button>
          <icon-button v-if="playing" @click="playing = !playing"
          iconName="bi-pause-fill" class="text-[25px]"></icon-button>
          <icon-button iconName="bi-skip-forward-fill" class="ml-5 text-[22px]"></icon-button>
        </div>

        <div class="flex justify-center w-full mt-2">
          <track-bar width="250" height="9" percent="55"></track-bar>
        </div>
      </div>

      <!-- Repeat button, visible on medium screens and centered vertically -->
      <icon-button iconName="bi-repeat" class="hidden md:flex justify-center items-center h-full text-xl"></icon-button>

      <!-- Volume control section -->
      <div class="flex items-center">
        <icon-button iconName="bi-volume-up-fill mr-2 text-2xl"></icon-button>
        <track-bar width="220" height="11" percent="55"></track-bar>
      </div>
      
    </div>
  </div>
</template>
