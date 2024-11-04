  <script setup lang="ts">
  import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import albumImage from '@/assets/images/album.jpeg';

  // Define the type for an song
  interface Song {
    name: string;
    length: number;
    listens: number;
    path: string;
  }

  // Define a reactive variable for songs
  const songs = ref<Song[]>([]);

  // Function to fetch songs from the API
  const fetchSongs = async () => {
    try {
      const response = await axios.get<Song[]>('http://localhost:5091/Songs');
      songs.value = response.data; // Update the reactive variable with fetched data
    } catch (error) {
      console.error('Error fetching songs:', error);
    }
  };

  // Fetch songs when the component is mounted
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
                :imagePath="albumImage"></album-preview>
              <!-- </div> --> 
            </div>
      </div>
  </PerfectScrollbar>
</template>
