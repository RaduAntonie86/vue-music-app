<script setup lang="ts"></script>

<template>
    <div class="flex justify-center">
        <p class="text-white font-arial mt-5 text-2xl">Username:</p>
    </div>
    <div class="flex justify-center">
        <form class="d-flex w-[30vh]" role="username">
            <input class="form-control me-2 input-rounded text-[#efd0d0] bg-custom" type="username" placeholder="Username" aria-label="Username" required>
        </form>
    </div>
    <div class="flex justify-center">
        <p class="text-white font-arial mt-5 text-2xl">Password:</p>
    </div>
    <div class="flex justify-center">
        <form class="d-flex w-[30vh]" role="password">
            <input class="form-control me-2 input-rounded text-[#efd0d0] bg-custom" minlength="8" type="password" placeholder="Password" aria-label="Password" required>
        </form>
    </div>
    <div class="flex justify-center mt-5">
        <input type="checkbox" id="checkbox"/>
        <div class="text-white font-arial ml-1">Remember me</div>
    </div>
    <div class="flex justify-center mt-5">
        <input type="submit" value="Login" class="text-white font-arial text-3xl font-semibold bg-[#362323] rounded-xl">
    </div>
</template>