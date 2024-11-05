<script setup lang="ts">
const props = defineProps<{
  iconName: string; 
}>();
</script>

<template>
  <button>
    <i :class="['bi', iconName, 'text-white']"></i>
  </button>
</template>
