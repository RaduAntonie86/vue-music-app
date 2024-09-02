<script setup lang="ts">
import { PerfectScrollbar } from 'vue3-perfect-scrollbar';
</script>

<template>
<div class="bg-[#362323] rounded-md">
    <div class="flex align-middle place-items-center p-4">
        <img class="rounded-3xl mr-4" src="../assets/images/album.jpeg" width="250" height="250">
        <div class="mt-[50px]">
            <div class="text-white text-lg font-arial mb-1">Playlist</div>
            <div class="text-white text-6xl font-semibold font-arial mb-1">Playlist Name</div>
            <div class="text-white text-2xl font-arial mb-2">Description</div>
            <div class="text-white text-lg font-arial">Made by:</div>
            <div class="flex align-middle place-items-center mb-2 mt-2">
                <img class="rounded-full mr-2.5" src="../assets/images/user.jpg" width="30" height="30">
                <div>
                    <div class="text-white text-md font-arial">Playlist Creator</div>
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
    <div class="grid grid-cols-5 p-2 mx-2 font-arial text-[#753E3E] font-bold">
        <div>#</div>
        <div class="col-span-2">Title</div>
        <div class="col-span-2">Album</div>
    </div>
    <div class="mx-2">
        <mat-divider></mat-divider>
    </div>
    <PerfectScrollbar class="min-h-[40vh] overflow-hidden">
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
        <div class="grid grid-cols-5 p-2 mx-2 font-arial font-bold">
            <div class="flex place-items-center font-normal">1</div>
            <div class="col-span-2">
                <div class="flex align-middle place-items-center mb-2 mt-2">
                    <img class="rounded-lg mr-2.5" src="../assets/images/album.jpeg" width="50" height="50">
                    <div>
                        <div class="text-white text-lg font-arial">Song Name</div>
                        <div class="text-white text-xs font-arial font-normal">Artist Name</div>
                    </div>
                </div>
            </div>
            <div class="col-span-2 flex place-items-center font-normal">Album Name</div>
        </div>
    </PerfectScrollbar>
</div>

</template>
