<script setup lang="ts">
import { useAttrs } from 'vue'

const props = defineProps<{
  iconName: string
}>()

const attrs = useAttrs()
</script>

<template>
  <button class="focus:outline-none">
    <i :class="['bi', iconName, attrs.class]" />
  </button>
</template>
