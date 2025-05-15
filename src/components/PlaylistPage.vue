<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import MusicPlayer from './MusicPlayer.vue';
</script>

<template>
    <div class="overflow-hidden mb-2 px-2 flex-grow">
        <div class="grid gap-2 text-white h-full 
                grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 xl:grid-cols-7">
        <div class="grid grid-rows-8 gap-2">
            <left-side-buttons></left-side-buttons>
            <left-sidebar class="row-span-7"></left-sidebar>
        </div>    
        <playlist-list class="sm:col-span-2 md:col-span-3 lg:col-span-5 xl:col-span-6"/>
        </div>
    </div>
    <music-player :songId="1" 
    :songName="'Unknown Song'"
    :imagePath="'Unknown Path'"
    :artistName="'Unknown Artist'"
    class="p-2"></music-player>
</template>