<script setup lang="ts">
import { computed, ref } from 'vue';

const props = defineProps<{
  width: number;
  height: number;
  percent: number;
}>();

// Create a ref to track the dynamic width
const containerWidth = ref(props.width);

// Dynamically calculate the progress bar width based on container width and percentage
const trackBarProgress = computed(() => (containerWidth.value * props.percent) / 100);
</script>

<template>
  <div class="track-bar-container" :style="{ maxWidth: `${props.width}px` }">
    <svg
      :width="'100%'"
      :height="props.height"
      :viewBox="`0 0 ${props.width} ${props.height}`"
      preserveAspectRatio="none"
    >
      <!-- Background rectangle -->
      <rect :width="props.width" :height="props.height" rx="7" ry="7" fill="gray" />
      
      <!-- Progress rectangle -->
      <rect :width="trackBarProgress" :height="props.height" rx="7" ry="7" fill="white" />
    </svg>
  </div>
</template>

<style scoped>
.track-bar-container {
  width: 100%;           /* Full width of container */
}
</style>
