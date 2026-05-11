<script setup lang="ts">

const props = defineProps<{
  width: number;
  height: number;
}>();
</script>

<template>
  <div
    ref="container"
    class="track-bar-container"
    :style="{ maxWidth: `${props.width}px` }"
  >
    <svg
      :width="'100%'"
      :height="props.height"
      :viewBox="`0 0 ${props.width} ${props.height}`"
      preserveAspectRatio="none"
    >
      <rect :width="props.width" :height="props.height" rx="7" ry="7" fill="#753E3E" />
    </svg>
  </div>
</template>


<style scoped>
.track-bar-container {
  width: 100%;
}
</style>
