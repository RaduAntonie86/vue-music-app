<script setup lang="ts">
import { onMounted, ref, reactive } from 'vue'
import { useMediaControls } from '@vueuse/core'

// Define the song object with properties that can be dynamic (e.g., title, artist, and id)
const currentSong = reactive({
  id: 1, // Example song ID; this could be dynamic
  title: 'Song Name',
  artist: 'Artist Name',
  src: '', // Initially empty; will be set when mounted
})

// Create a ref for the audio element
const audio = ref<HTMLAudioElement | null>(null)

// Use media controls with the song's src property
const { playing, currentTime, duration, volume } = useMediaControls(audio, {
  src: currentSong.src,  // Set the source dynamically
})

// Change initial media properties when the component is mounted
onMounted(() => {
  currentSong.src = `http://localhost:5091/Songs/stream/${currentSong.id}`; // Set the song source dynamically based on song ID
})

// Function to play the song
function playSong() {
  if (audio.value) {
    audio.value.play().then(() => {
      playing.value = true;
      //console.log("Audio is playing");
    }).catch((err) => {
      console.error("Error playing audio", err);
    });
  }
}

// Function to pause the song
function pauseSong() {
  if (audio.value) {
    playing.value = false;
    audio.value.pause();
    //console.log("Audio is paused");
  }
}

// Function to handle previous song action
function previousSong() {
  if (audio.value) {
    audio.value.currentTime = 0; // Reset to the beginning
  }
  // You can set currentSong.id to a previous song ID if you have a list of songs
}

function togglePlayPause() {
  if (playing.value) {
    pauseSong();
  } else {
    playSong();
  }
}

// Function to handle audio errors
function handleAudioError() {
  console.error('Error loading audio');
}
</script>

<template>
  <div class="bg-[#362323]">
    <div class="grid grid-cols-3 md:grid-cols-5 gap-2 text-white items-center">

      <div class="flex align-middle items-center">
        <img class="rounded-3xl mr-[10px]" src="../assets/images/album.jpeg" width="70" height="70">
        <div> 
          <div class="text-white text-lg font-arial">{{ currentSong.title }}</div>
          <div class="text-white text-xs font-arial">{{ currentSong.artist }}</div>
        </div> 
      </div>

      <button class="hidden md:flex justify-center items-center h-full">
        <i class="bi bi-shuffle text-xl text-white"></i>
      </button>

      <div class="flex flex-col items-center justify-center h-full">
        <div class="flex items-center">
          <icon-button @click="previousSong"
          iconName="bi-skip-backward-fill" class="mr-5 text-[22px]"></icon-button>
          <icon-button @click="togglePlayPause"
          :iconName="playing ? 'bi-pause-fill' : 'bi-play-fill'" 
          class="text-[25px]"></icon-button>
          <icon-button iconName="bi-skip-forward-fill" class="ml-5 text-[22px]"></icon-button>
        </div>

        <div class="flex justify-center w-full mt-2">
          <track-bar width="250" height="9" percent="55"></track-bar>
        </div>
      </div>

      <icon-button iconName="bi-repeat" class="hidden md:flex justify-center items-center h-full text-xl"></icon-button>

      <div class="flex items-center">
        <icon-button iconName="bi-volume-up-fill mr-2 text-2xl"></icon-button>
        <track-bar 
          width="220" 
          height="11" 
          :percent="volume * 100" 
          @input="(newVolume: number) => { volume = newVolume / 100; }">
        </track-bar>
      </div>
    </div>
  </div>

  <audio ref="audio" :src="currentSong.src" @error="handleAudioError"></audio>
</template>
