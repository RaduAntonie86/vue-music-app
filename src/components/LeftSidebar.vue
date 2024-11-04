<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar'
import playlistImage from '../assets/images/album.jpeg'
</script>

<style>
@import 'vue3-perfect-scrollbar/style.css';

</style>

<template>
<PerfectScrollbar class="bg-[#362323] rounded-md p-1">
    <playlist-preview 
      playlistName="Playlist Name" 
      description="Description" 
      :imagePath=playlistImage>
    </playlist-preview>
</PerfectScrollbar>
</template>
