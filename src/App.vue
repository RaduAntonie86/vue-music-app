<script setup lang="ts">
</script>

<template>
  <div class="bg-[#21201e] flex flex-col h-screen">
    <div class="grid grid-cols-2">
      <RouterLink to="/" class="font-arial text-white no-underline mb-3 text-5xl font-semibold text-left">App Name</RouterLink>
      <RouterLink to="/login" class="font-arial text-white no-underline mb-3 text-5xl font-semibold text-right">Log In</RouterLink>
    </div>
    <RouterView/>
  </div>
</template>
