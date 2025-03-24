<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
</script>

<template>
<div class="bg-[#362323] rounded-md overflow-hidden">
    <div class="flex align-middle place-items-center p-4">
        <img class="rounded-3xl mr-4" src="../assets/images/album.jpeg" width="250" height="250">
        <div class="mt-[50px]">
            <div class="text-white text-lg font-arial mb-1">Album</div>
            <div class="text-white text-6xl font-semibold font-arial mb-1">Album Name</div>
            <div class="flex align-middle place-items-center mb-2 mt-2">
                <img class="rounded-full mr-2.5" src="../assets/images/user.jpg" width="30" height="30">
                <div>
                    <div class="text-white text-md font-arial mr-1">Artist,</div>
                </div>
                <div>
                    <div class="text-white text-md font-arial">Year</div>
                </div>
            </div>
        </div>
    </div>
    <div class="mx-2">
        <div class="flex justify-between items-center">
            <div class="rounded-full ml-1 bg-[#753E3E] size-12 flex place-content-center place-items-center">
                <i class="fas fa-play text-2xl"></i>
            </div>
        </div>
    </div>    
    <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial text-[#753E3E] font-bold">
        <div>#</div>
        <div class="col-span-2">Title</div>
        <div class="col-span-2">Plays</div>
    </div>
    <div class="mx-2">
        <mat-divider></mat-divider>
    </div>
    <PerfectScrollbar class="overflow-hidden min-h-[43vh]">
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
        <div class="grid grid-cols-5 gap-4 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Arist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Number of plays</div>
        </div>
    </PerfectScrollbar>
</div>

</template>
