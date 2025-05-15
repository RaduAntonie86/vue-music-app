<script setup lang="ts">
const props = defineProps<{
  playlistName: string;
  description: string;
  imagePath: string;
}>();
</script>

<template>
  <button>
    <div class="flex align-middle place-items-center mb-2 mt-2 text-arial">
        <img class="rounded-3xl mr-2.5" :src="props.imagePath" width="70" height="70">
        <div>
            <div class="text-white text-lg font-arial">{{props.playlistName}}</div>
            <div class="text-white text-xs font-arial">{{props.description}}</div>
        </div>
    </div>
  </button>
</template>