<script setup lang="ts"></script>

<template>
<div class="grid-cols-3 grid bg-[#362323]">
    <div class="flex align-middle place-items-center">
        <img class="rounded-3xl p-1" src="../assets/images/album.jpeg" width="70" height="70" style="margin-right: 10px;">
        <div>
          <div class="text-white text-lg font-arial">Song Name</div>
          <div class="text-white text-xs font-arial">Artist Name</div>
        </div>
    </div>
    <div class="justify-center items-center flex">
        <i class="bi bi-shuffle mr-16 text-xl text-white"></i>
        <i class="bi bi-skip-backward-fill mr-5 text-[22px] text-white"></i>
        <i class="bi bi-play-fill text-[25px]" style="color: white;"></i>
        <i class="bi bi-skip-forward-fill ml-5 text-[22px] text-white"></i>
        <i class="bi bi-repeat ml-16 text-xl text-white"></i>
    </div>
    <div class="grid justify-items-end place-items-center p-2">
        <svg width="225" height="11">
            <rect width="225" height="11" rx="7" ry="7" fill="gray"/>
            <rect width="150" height="11" rx="7" ry="7" fill="white"/>
        </svg>
    </div>
</div>
</template>
