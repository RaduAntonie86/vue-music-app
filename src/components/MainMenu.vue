<script setup lang="ts"></script>

<template>
<div class="grid grid-cols-7 gap-2 text-white">
    <div class="grid grid-rows-11 gap-2">
        <left-side-buttons class="row-span-1"></left-side-buttons>
        <left-sidebar class="row-span-10 min-h-[75vh]"></left-sidebar>
    </div>
    <div class="col-span-6">
        <main-page class="min-h-[84.8vh]"></main-page>
        <!-- <search-page class="min-h-[84.8vh]"></search-page> -->
        <!-- <playlist-page class="min-h-[84.8vh]"></playlist-page> -->
        <!-- <album-page class="min-h-[84.8vh]"></album-page> -->
    </div>
</div>
</template>
