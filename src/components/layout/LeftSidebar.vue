<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'
import playlistImage from '../../assets/images/album.jpeg'
import axios from 'axios';
import { onMounted, ref } from 'vue';
import router from '@/router';

  const playlists = ref<Playlist[]>([]);

  const fetchPlaylists = async () => {
    try {
      const response = await axios.get<Playlist[]>('http://localhost:5091/Playlist');
      playlists.value = response.data;
    } catch (error) {
      console.error('Error fetching playlists:', error);
    }
  };

  const handlePlaylistClick = (id: number) => {
    console.log('Clicked playlist ID:', id);
    router.push({ name: 'playlist', params: { id: id.toString() } });
  };

  onMounted(() => {
    fetchPlaylists();
  });
</script>

<style>
@import 'vue3-perfect-scrollbar/style.css';

</style>

<template>
<PerfectScrollbar class="bg-[#362323] rounded-md p-1">
    <div v-for="(playlist) in playlists" :key="playlist.id" @click="handlePlaylistClick(playlist.id)">
      <playlist-preview 
        :playlistName="playlist.name"
        :description="playlist.description" 
        :imagePath="playlist.image_path ? playlist.image_path : playlistImage">
      </playlist-preview>
    </div>
</PerfectScrollbar>
</template>
