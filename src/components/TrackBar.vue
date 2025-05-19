<script setup lang="ts">
import { computed, ref } from 'vue';

const props = defineProps<{
  width: number;
  height: number;
  percent: number;
}>();

const emit = defineEmits<{
  (e: 'input', value: number): void;
}>();

const container = ref<HTMLDivElement | null>(null);
const containerWidth = ref(props.width);

const trackBarProgress = computed(() => (containerWidth.value * props.percent) / 100);

function handleClick(event: MouseEvent) {
  if (!container.value) return;

  const rect = container.value.getBoundingClientRect();
  const clickX = event.clientX - rect.left;
  const percent = (clickX / rect.width) * 100;

  emit('input', Math.max(0, Math.min(100, percent)));
}
</script>

<template>
  <div
    ref="container"
    class="track-bar-container"
    :style="{ maxWidth: `${props.width}px` }"
    @click="handleClick"
  >
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
