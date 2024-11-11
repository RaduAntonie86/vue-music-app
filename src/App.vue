<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import MusicPlayer from './components/MusicPlayer.vue';

  interface SongData {
  id: number;
  name: string;
  length: number;
  listens: number;
  path: string;
  }

  // Reactive state
  const songId = ref<number>(1);
  const songData = ref<SongData | null>(null);
  const songUrl = ref<string>(''); // URL for the audio source

  // Fetch song data on component mount or when songId changes
  const fetchSongData = async () => {
    try {
      const response = await fetch(`http://localhost:5091/Songs/${songId.value}`);
      if (!response.ok) {
        throw new Error(`Error fetching song: ${response.statusText}`);
      }

      const data: SongData = await response.json();
      songData.value = data;
      songUrl.value = `http://localhost:5091/Songs/stream/${data.id}`; // Path to stream song based on id
    } catch (error) {
      console.error("Failed to fetch song data:", error);
    }
  };

  // Fetch song data when the component is mounted
  onMounted(fetchSongData);

  // Watch for changes to songId and fetch new data
  watch(songId, fetchSongData);
</script>

<template>
  <div class="bg-[#21201e] flex flex-col h-screen">
    <h1 class="font-arial text-white mb-3">App Name</h1>
    <div class="overflow-hidden mb-2 px-2 flex-grow">
      <main-menu></main-menu>
    </div>
    <music-player :songId="songData?.id || 1" 
    :songName="songData?.name || 'Unknown Song'"
    :imagePath="'Unknown Path'"
    :artistName="'Unknown Artist'"
    class="p-2"></music-player>
  </div>
</template>
