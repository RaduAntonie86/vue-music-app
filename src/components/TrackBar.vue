<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps<{
  width: number;
  height: number;
  percent: number;
}>();

const trackBarProgress = computed(() => (props.width * props.percent) / 100);
</script>

<template>
<svg :width="props.width" :height="props.height">
    <rect :width="props.width" :height="props.height" rx="7" ry="7" fill="gray"/>
    <rect :width="trackBarProgress" :height="props.height" rx="7" ry="7" fill="white"/>
</svg>
</template>
