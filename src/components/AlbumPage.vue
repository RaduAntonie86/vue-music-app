<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import MusicPlayer from './MusicPlayer.vue';

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
    <div class="overflow-hidden mb-2 px-2 flex-grow">
        <div class="grid gap-2 text-white h-full 
                grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 xl:grid-cols-7">
        <div class="grid grid-rows-8 gap-2">
            <left-side-buttons></left-side-buttons>
            <left-sidebar class="row-span-7"></left-sidebar>
        </div>   
        <album-list class="sm:col-span-2 md:col-span-3 lg:col-span-5 xl:col-span-6"/>
        </div>
    </div>
    <music-player :songId="songData?.id || 1" 
    :songName="songData?.name || 'Unknown Song'"
    :imagePath="'Unknown Path'"
    :artistName="'Unknown Artist'"
    class="p-2"></music-player>
</template>