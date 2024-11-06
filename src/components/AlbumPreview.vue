<script setup lang="ts">
const props = defineProps<{
  albumName: string;
  artistName: string;
  imagePath: string;
}>();
</script>

<template>
<div class="flex flex-col items-center">
    <img class="rounded-3xl min-h-[100px] min-w-[100px]" :src="props.imagePath" width="120" height="120" style="margin-bottom: 5px;">
    <div class="text-white text-lg font-arial">{{ props.albumName }}</div>
    <div class="text-white text-sm font-arial">{{ props.artistName }}</div>
</div>
</template>
