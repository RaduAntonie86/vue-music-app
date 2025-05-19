<script setup lang="ts">
import { computed, ref } from 'vue';

const props = defineProps<{
  width: number;
  height: number;
  percent: number;
}>();

const containerWidth = ref(props.width);

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
      <rect :width="props.width" :height="props.height" rx="7" ry="7" fill="gray" />
      
      <rect :width="trackBarProgress" :height="props.height" rx="7" ry="7" fill="white" />
    </svg>
  </div>
</template>

<style scoped>
.track-bar-container {
  width: 100%;
}
</style>
