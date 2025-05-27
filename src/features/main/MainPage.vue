  <script setup lang="ts">
  import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
  import { ref, onMounted } from 'vue';
  import axios from 'axios';

  const songs = ref<Song[]>([]);

  const fetchSongs = async () => {
    try {
      const response = await axios.get<Song[]>('http://localhost:5091/Song');
      songs.value = response.data;
    } catch (error) {
      console.error('Error fetching songs:', error);
    }
  };

  onMounted(() => {
    fetchSongs();
  });
  </script>

<template>
  <PerfectScrollbar class="bg-[#362323] rounded-md">
    <div class="font-arial text-white ml-6 mt-4">
      <p class="text-3xl mb-4">Try something new</p>
      <div class="w-full flex justify-center max-w-screen w-full grid grid-cols-7 gap-2">
              <!-- <div v-for="(song, index) in songs" :key="index" class="flex items-center justify-center mb-2 mt-2"> -->
                <album-preview 
                albumName="Album Name" 
                artistName="Artist Name"
                imagePath="images/albums/album.jpeg"></album-preview>
              <!-- </div> --> 
            </div>
      </div>
  </PerfectScrollbar>
</template>
